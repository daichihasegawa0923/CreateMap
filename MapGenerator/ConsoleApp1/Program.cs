using System;
using MapGenerator.Code.Generator;

namespace MapGenerator.Code
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = RandomMapGenerator.GenerateMap(5, new int[20, 20]);

            RandomMapGenerator.DebugConsole(map); 
        }
    }
}
