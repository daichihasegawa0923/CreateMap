using System.IO;

namespace MapGenerator.Code
{
    public class MapDebugger
    {
        public static void MapDebugByText(int[,] map,string fileName = "map.txt")
        {
            using (var sw = new StreamWriter(fileName))
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    var str = "";
                    for (int j = 0; j < map.GetLongLength(0); j++)
                    {
                        str += map[i, j].ToString();
                    }
                    sw.WriteLine(str);
                }
            }
        }
    }
}
