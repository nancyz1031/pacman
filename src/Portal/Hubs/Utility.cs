using Editor.Hubs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Editor.Hubs
{
    public class Utility
    {
        static Random r = new Random();

        public static Position GetRandomPosition()
        {
            var variables = Variables.Instance;
            return new Position()
            {
                X = r.Next(0, variables.WorldWidth),
                Y = r.Next(0, variables.WorldHeight)
            };
        }

    }
}
