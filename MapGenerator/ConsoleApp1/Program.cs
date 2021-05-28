using System;
using MapGenerator.Code.Generator;

namespace MapGenerator.Code
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = RandomMapGenerator.GenerateMap(10, new int[60, 70]);

            RandomMapGenerator.DebugConsole(map); 
        }
    }
}
