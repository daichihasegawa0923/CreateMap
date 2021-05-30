using System;
using System.IO;
using MapGenerator.Code.Generator;

namespace MapGenerator.Code
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = RandomMapGenerator.GenerateMap(10, new int[50, 50]);

            Directory.CreateDirectory("maps");
            for(var i = 0; i < 2000; i++)
            {
                MapDebugger.MapDebugByText(map,"maps/map_"+i+".txt");
            }
        }
    }
}
