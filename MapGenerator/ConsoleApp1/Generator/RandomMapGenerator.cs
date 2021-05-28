using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator.Code.Generator
{
    public class RandomMapGenerator
    {
        public static int[,] GenerateMap(
            int count,
            int[,] map = null,
            Rect nextRect = null,
            List<Point[]> dividions = null,
            List<Rect> rects = null,
            bool? isX = null
            )
        {
            if (count == 0)
            {
                return CreateRoom(map, rects,dividions);
            }

            count--;

            if (map == null)
                throw new Exception();

            if (nextRect == null)
                nextRect = new Rect(0, 0, map.GetLength(1), map.GetLength(0));

            if (dividions == null)
                dividions = new List<Point[]>();

            if (rects == null)
                rects = new List<Rect>();


            isX = isX == null ? new Random().Next(0, 2) == 0 : !isX.Value;

            var dividerStartPoint = new Point(
                isX.Value ? new Random().Next(nextRect.StartPosition.x, (nextRect.StartPosition.x + nextRect.Size.x) - 1) : nextRect.StartPosition.x,
                !isX.Value ? new Random().Next(nextRect.StartPosition.y, (nextRect.StartPosition.y + nextRect.Size.y) - 1) : nextRect.StartPosition.y
                );

            var Xcount = isX.Value ? dividerStartPoint.x : nextRect.StartPosition.x;
            var Ycount = !isX.Value ? dividerStartPoint.y : nextRect.StartPosition.y;


            var divide = new Point[isX.Value ? nextRect.Size.y : nextRect.Size.x ];

            for (var i = 0; i < (isX.Value ? nextRect.Size.y : nextRect.Size.x); i++)
            {
                divide[i] = new Point(Xcount+1, Ycount+1);

                if (isX.Value)
                    Ycount++;
                else
                    Xcount++;
            }

            Rect leargeRect;
            Rect smallRect;

            if (isX.Value)
            {
                var isLeftSmall = dividerStartPoint.x - nextRect.StartPosition.x  <  nextRect.Size.x / 2;

                var leftSideRect = new Rect(nextRect.StartPosition.x, nextRect.StartPosition.y, dividerStartPoint.x - nextRect.StartPosition.x - 1, nextRect.Size.y);
                var rightSideRect = new Rect(dividerStartPoint.x + 1, nextRect.StartPosition.y, (nextRect.StartPosition.x + nextRect.Size.x) - dividerStartPoint.x - 1, nextRect.Size.y);

                smallRect = isLeftSmall ? leftSideRect : rightSideRect;
                leargeRect = isLeftSmall ? rightSideRect : leftSideRect;
            }
            else
            {
                var isUpSmall = dividerStartPoint.y - nextRect.StartPosition.y < nextRect.Size.y / 2;

                var upSideRect = new Rect(nextRect.StartPosition.x, nextRect.StartPosition.y, nextRect.Size.x, dividerStartPoint.y - nextRect.StartPosition.y - 1);
                var downSideRect = new Rect(nextRect.StartPosition.x,dividerStartPoint.y + 1, nextRect.Size.x, (nextRect.StartPosition.y + nextRect.Size.y) - dividerStartPoint.y - 1);

                smallRect = isUpSmall ? upSideRect : downSideRect;
                leargeRect = isUpSmall ? downSideRect : upSideRect;
            }


            if (smallRect.IsValidSize)
            {
                rects.Add(smallRect);
                dividions.Add(divide);
            }

            return GenerateMap(count, map, leargeRect, dividions, rects,isX:isX);
        }

        private static int[,] CreateRoom(int[,] map, List<Rect> rects,List<Point[]> dividions)
        {

            if (rects == null || rects.Count == 0)
            {
                return GenerateMap(5, map);
            }

            // 通路を作る
            foreach (var divide in dividions)
            {
                for(var i = 0; i < divide.Length; i++)
                {
                   map[divide[i].y - 1, divide[i].x - 1] = 1;
                }
                DebugConsole(map);
            }

            foreach (var rect in rects)
            {
                for (var i = rect.StartPosition.y + 1; i < rect.StartPosition.y + rect.Size.y - 1; i++)
                {
                    for (var j = rect.StartPosition.x + 1; j < rect.StartPosition.x + rect.Size.x - 1; j++)
                    {
                        map[i, j] = 2;
                    }
                }

                DebugConsole(map);
            }

            // 周りを壁にする
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    if (i == 0 || i == map.GetLength(0) - 1 || j == 0 || j == map.GetLength(1) - 1)
                        map[i, j] = 0;
                }
            }

            DebugConsole(map);

            return map;
        }

        public static void DebugConsole(int[,] map)
        {
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine("");
            }

            Console.WriteLine("");
        }
    }

    public class Rect
    {
        public readonly Point StartPosition;
        public readonly Point Size;


        private static int smallestSize = 5;
        public bool IsValidSize { get => this.Size.x > smallestSize && this.Size.y > smallestSize; }

        public Rect(int startPositionX, int startPositionY, int sizeX, int sizeY)
        {
            StartPosition = new Point(startPositionX, startPositionY);
            Size = new Point(sizeX, sizeY);
        }
    }

    public class Point
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
