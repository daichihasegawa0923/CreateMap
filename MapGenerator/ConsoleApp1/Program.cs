using System;
using MapGenerator.Code.Generator;

namespace MapGenerator.Code
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = RandomMapGenerator.GenerateMap(20, new int[100, 100]);
        }
    }
}
