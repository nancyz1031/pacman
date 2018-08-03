using Editor.Hubs.Model;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Editor.Hubs
{
    public class PacmanHub : Hub
    {
        private const string SystemMessageFunc = "SystemMessage";
        private const string UpdateRanksFunc = "UpdateRanks";
        private const string UpdateDotsFunc = "UpdateDots";
        private const string StartGameFunc = "StartGame";
        private const string PlayerMoveToFunc = "PlayerMoveTo";
        private const string UpdatePlayersFunc = "UpdatePlayers";

        private static object SyncRoot = new object();
        private static object RankSyncRoot = new object();
        private static World world = new World();

        #region From client

        public void PlayerJoin(string userName)
        {
            var id = Guid.NewGuid().ToString();
            SetPlayerId(id);
            var user = new Player(id, userName);
            world.Players[user.Id] = user;
            Start();
            TryFillDotsToWorld();
            RefreshRanks();
            RefreshPlayers();
        }

        public void Start()
        {
            if (world.Players.TryGetValue(GetPlayerId(), out Player player))
            {
                player.Score = 0;
                player.Position = Utility.GetRandomPosition();
                SendSystemMessage($"{player.Name} joined game");
                Clients.Caller.SendAsync(StartGameFunc, player.Id, world);
            }
        }

        public void PlayerMoveTo(Position position)
        {
            var playerId = GetPlayerId();
            world.Players.TryGetValue(playerId, out Player player);
            player.Position = position;
            Clients.Others.SendAsync(PlayerMoveToFunc, playerId, position);
            var dots = world.Dots.Values.ToArray();
            foreach (var dot in dots)
            {
                if (dot.Position.Equals(position))
                {
                    PlayerTryEatDot(playerId, dot.Id);
                }
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            this.PlayerLeave(GetPlayerId());
            return base.OnDisconnectedAsync(exception);
        }

        #endregion

        private void SendSystemMessage(string message)
        {
            Clients.All.SendAsync(SystemMessageFunc, message);
        }

        private void RefreshRanks()
        {
            lock (RankSyncRoot)
            {
                var oldRanks = JsonConvert.SerializeObject(world.Ranks);
                world.Ranks = world.Players.Values.ToList()
                    .Select(user => new Rank() { Id = user.Id, UserName = user.Name, ColorIndex = user.Index, Score = user.Score })
                    .OrderByDescending(rank => rank.Score)
                    .ToList();
                var newRanks = JsonConvert.SerializeObject(world.Ranks);

                if (oldRanks != newRanks)
                {
                    Clients.All.SendAsync(UpdateRanksFunc, world.Ranks);
                }
            }
        }

        private void RefreshPlayers()
        {
            Clients.All.SendAsync(UpdatePlayersFunc, world.Players);
        }

        private void SetPlayerId(string playerId)
        {
            this.Context.Items["id"] = playerId;
        }

        private string GetPlayerId()
        {
            var id = this.Context.Items["id"];
            return id?.ToString();
        }

        private void PlayerLeave(string playerId)
        {
            if (playerId == null)
            {
                // Haven't login
                return;
            }

            world.Players.TryRemove(playerId, out Player player);
            if (player != null)
            {
                SendSystemMessage($"{player.Name} left game");
                RefreshRanks();
                RefreshPlayers();
            }
        }

        private void PlayerTryEatDot(string playerId, string dotId)
        {
            if (!world.Players.TryGetValue(playerId, out Player user))
            {
                return;
            }

            lock (SyncRoot)
            {
                if (!world.Dots.ContainsKey(dotId))
                {
                    // Other user eat this dot already
                    return;
                }

                SendSystemMessage($"{user.Name} eat a dot!");
                user.Score++;
                world.Dots.TryRemove(dotId, out Dot dot);
                TryFillDotsToWorld();
                RefreshRanks();
            }
        }

        private void TryFillDotsToWorld()
        {
            if (world.Dots.Count >= world.Variables.MaxDot)
            {
                return;
            }

            lock (SyncRoot)
            {
                while (world.Dots.Count < world.Variables.MaxDot)
                {
                    var dot = new Dot(Utility.GetRandomPosition());
                    world.Dots[dot.Id] = dot;
                }

                if (Clients != null)
                {
                    Clients.All.SendAsync(UpdateDotsFunc, world.Dots.Values.ToList());
                }
            }
        }
    }
}
