namespace Editor.Hubs.Model
{
    public class Variables
    {
        public int MaxDot { get; } = 6;
        public int Step { get; } = 20;
        public int WorldWidth { get; } = 40;
        public int WorldHeight { get; } = 30;

        public static Variables Instance { get; } = new Variables();
    }
}
