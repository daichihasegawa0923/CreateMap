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

            var isX = new Random().Next(0, 2) == 0;

            var startPoint = new Point(
                isX ? new Random().Next(0, map.GetLength(1) - 1) : 0,
                !isX ? new Random().Next(0, map.GetLength(0) - 1) : 0);

            var Xcount = isX ? startPoint.x : 0;
            var Ycount = !isX ? startPoint.y : 0;

            var dividions = new List<Point[]>();
            var rects = new List<Rect>();


            var divide = new Point[isX ? map.GetLength(0) : map.GetLength(1)];

            for (var i = 0; i < (isX ? map.GetLength(0):map.GetLength(1)); i++)
            {
                map[Ycount, Xcount] = 1;
                divide[isX ? Ycount : Xcount] = new Point(Xcount, Ycount);

                if (isX)
                    Ycount++;
                else
                    Xcount++;
            }

            Rect leargeRect;
            Rect smallRect;
            if (isX)
            {
                var isLeft = map.GetLength(1) - startPoint.x > map.GetLength(1) / 2;
                if (isLeft)
                {
                    smallRect = new Rect(0, 0, startPoint.x - 1, map.GetLength(0));
                    leargeRect = new Rect(startPoint.x + 1, 0, map.GetLength(1) - startPoint.x - 1, map.GetLength(0));
                }
                else
                {
                    leargeRect = new Rect(0, 0, startPoint.x - 1, map.GetLength(0));
                    smallRect = new Rect(startPoint.x + 1, 0, map.GetLength(1) - startPoint.x - 1, map.GetLength(0));
                }
            }
            else
            {
                var isUp = map.GetLength(0) - startPoint.y > map.GetLength(0) / 2;
                if (isUp)
                {
                    smallRect = new Rect(0, 0, map.GetLength(1), startPoint.y - 1);
                    leargeRect = new Rect(0, startPoint.y + 1, map.GetLength(1), map.GetLength(0) - startPoint.y - 1);
                }
                else
                {
                    leargeRect = new Rect(0, 0, map.GetLength(1), startPoint.y - 1);
                    smallRect = new Rect(0, startPoint.y + 1, map.GetLength(1), map.GetLength(0) - startPoint.y - 1);
                }
            }

            dividions.Add(divide);

            if(smallRect.IsValidSize)
                rects.Add(smallRect);

            return map;
        }

        public static int[,] GenerateMap(
            int x, int y,int count,
            int[,] map = null,
            Rect nextRect = null,
            List<Point[]> dividions = null,
            List<Rect> rects = null)
        {
            if (count == 0)
                return map;

            count--;

            if (map == null)
                map = new int[y, x];

            if (nextRect == null)
                nextRect = new Rect(0, 0, map.GetLength(1), map.GetLength(0));

            if (dividions == null)
                dividions = new List<Point[]>();

            if (rects == null)
                rects = new List<Rect>();


            var isX = new Random().Next(0, 2) == 0;

            var startPoint = new Point(
                isX ? new Random().Next(nextRect.StartPosition.x, (nextRect.StartPosition.x + nextRect.Size.x) - 1) : nextRect.StartPosition.x,
                !isX ? new Random().Next(nextRect.StartPosition.y, (nextRect.StartPosition.y + nextRect.Size.y) - 1) : nextRect.StartPosition.y);

            var Xcount = isX ? startPoint.x : nextRect.StartPosition.x;
            var Ycount = !isX ? startPoint.y : nextRect.StartPosition.y;


            return map;
        }
    }

    public class Rect
    {
        public readonly Point StartPosition;
        public readonly Point Size;


        private static int smallestSize = 6;
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
