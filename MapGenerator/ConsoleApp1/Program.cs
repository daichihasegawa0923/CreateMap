using System;
using MapGenerator.Code.Generator;

namespace MapGenerator.Code
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = RandomMapGenerator.GenerateMap(4, new int[30, 50]);

            RandomMapGenerator.DebugConsole(map); 
        }
    }
}
