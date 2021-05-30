using System;
using System.IO;
using MapGenerator.Code.Generator;

namespace MapGenerator.Code
{
    class Program
    {
        static void Main(string[] args)
        {

            Directory.CreateDirectory("maps");
            for(var i = 0; i < 200; i++)
            {
                var map = RandomMapGenerator.GenerateMap(15, new int[50, 50]);
                MapDebugger.MapDebugByText(map,"maps/map_"+i+".txt");
            }
        }
    }
}
