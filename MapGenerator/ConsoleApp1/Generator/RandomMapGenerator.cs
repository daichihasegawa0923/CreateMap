using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator.Code.Generator
{
    public class RandomMapGenerator
    {
        public static int[,] Generate(int x, int y)
        {
            var map = new int[y, x];

            var isX = new Random().Next(0, 1) == 0;

            var startPoint = new Point(
                isX ? new Random().Next(0, map.GetLength(1) - 1) : 0,
                !isX ? new Random().Next(0, map.GetLength(0) - 1) : 0);

            var Xcount = isX ? startPoint.x : 0;
            var Ycount = !isX ? startPoint.y : 0;

            var dividions = new List<int[][]>();


            var divide = new int[isX ? map.GetLength(0) : map.GetLength(1)][];

            for (var i = 0; i < (isX ? map.GetLength(0):map.GetLength(1)); i++)
            {
                map[Ycount, Xcount] = 1;
                divide[isX?Ycount:Xcount] = new int[]{Ycount,Xcount};

                if (isX)
                    Ycount++;
                else
                    Xcount++;
            }

            dividions.Add(divide);

            return map;
        }

        class Point
        {
            public readonly int x;
            public readonly int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
    }
}
