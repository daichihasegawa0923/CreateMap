using System;
using MapGenerator.Code.Generator;

namespace MapGenerator.Code
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = RandomMapGenerator.Generate(10, 10);
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for(var j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine("");
            }
        }
    }
}
