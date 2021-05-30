using System;
using System.Collections.Generic;

namespace MapGenerator.Code.Generator
{
    /// <summary>
    ///  ランダムなマップを生成するクラス（区域分割法）
    /// </summary>
    public class RandomMapGenerator
    {
        public static readonly int maxSize = 10;

        /// <summary>
        /// ランダムなマップを生成します
        /// </summary>
        /// <param name="count">作りたい部屋数</param>
        /// <param name="map">整数の二次元配列</param>
        /// <param name="nextRect">次に分割する部屋</param>
        /// <param name="dividions">通路を管理する配列</param>
        /// <param name="rects">分割された部屋を管理する配列</param>
        /// <param name="isX">縦に分割するか、横に分割するか</param>
        /// <returns>ランダムなマップ</returns>
        private static int[,] GenerateMap(
            int count,
            int[,] map = null,
            Rect nextRect = null,
            List<Point[]> dividions = null,
            List<Rect> rects = null
            )
        {
            // カウントが０になったら処理を止める
            if (count == 0)
            {
                return CreateRoom(map, rects,dividions);
            }

            count--;

            // 引数にマップがない場合は、例外を投げる
            if (map == null)
                throw new Exception();

            
            if (nextRect == null)
                nextRect = new Rect(0, 0, map.GetLength(1), map.GetLength(0));

            if (dividions == null)
                dividions = new List<Point[]>();

            if (rects == null)
                rects = new List<Rect>();

            // 縦割りと横わりを再帰するたびに切り替える
            bool? isX = new Random().Next(0, 2) == 0;

            //  通路を生成する初期位置をを決める
            var dividerStartPoint = new Point(
                isX.Value ? new Random().Next(nextRect.StartPosition.x, (nextRect.StartPosition.x + nextRect.Size.x)) : nextRect.StartPosition.x,
                !isX.Value ? new Random().Next(nextRect.StartPosition.y, (nextRect.StartPosition.y + nextRect.Size.y)) : nextRect.StartPosition.y
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

                var leftSideRect = new Rect(nextRect.StartPosition.x, nextRect.StartPosition.y, dividerStartPoint.x - nextRect.StartPosition.x, nextRect.Size.y);
                var rightSideRect = new Rect(dividerStartPoint.x + 1, nextRect.StartPosition.y, (nextRect.StartPosition.x + nextRect.Size.x) - dividerStartPoint.x - 1, nextRect.Size.y);

                smallRect = isLeftSmall ? leftSideRect : rightSideRect;
                leargeRect = isLeftSmall ? rightSideRect : leftSideRect;
            }
            else
            {
                var isUpSmall = dividerStartPoint.y - nextRect.StartPosition.y < nextRect.Size.y / 2;

                var upSideRect = new Rect(nextRect.StartPosition.x, nextRect.StartPosition.y, nextRect.Size.x, dividerStartPoint.y - nextRect.StartPosition.y);
                var downSideRect = new Rect(nextRect.StartPosition.x,dividerStartPoint.y + 1, nextRect.Size.x, (nextRect.StartPosition.y + nextRect.Size.y) - dividerStartPoint.y - 1);

                smallRect = isUpSmall ? upSideRect : downSideRect;
                leargeRect = isUpSmall ? downSideRect : upSideRect;
            }

            smallRect = new Rect(smallRect.StartPosition.x, smallRect.StartPosition.y,
                smallRect.Size.x > maxSize ? maxSize : smallRect.Size.x,
                smallRect.Size.y > maxSize ? maxSize : smallRect.Size.y);

            if (smallRect.IsValidSize)
            {
                rects.Add(smallRect);
                dividions.Add(divide);
            }


            return GenerateMap(count, map, leargeRect, dividions, rects);
        }

        /// <summary>
        /// ランダムなマップを生成します
        /// </summary>
        /// <param name="count">作りたい部屋数</param>
        /// <param name="map">整数の二次元配列</param>
        /// <returns>ランダムなマップ</returns>
        public static int[,] GenerateMap(
            int count,
            int[,] map
            )
        {

            // 引数にマップがない場合は、例外を投げる
            if (map == null)
                throw new Exception();

            var nextRect = new Rect(1, 1, map.GetLength(1) - 1, map.GetLength(0) - 1);
            
            var  dividions = new List<Point[]>();

            var rects = new List<Rect>();

            bool? isX = new Random().Next(0, 2) == 0;

            return GenerateMap(count, map, nextRect, dividions, rects);
        }

        /// <summary>
        /// 受け取った情報をもとに部屋を生成します。
        /// </summary>
        /// <param name="map">整数の二次元配列</param>
        /// <param name="rects">部屋の情報</param>
        /// <param name="dividions">通路の情報</param>
        /// <returns>部屋が生成された整数の二次元配列</returns>
        private static int[,] CreateRoom(int[,] map, List<Rect> rects,List<Point[]> dividions)
        {
            rects = ResizeRects(rects);

            if (rects == null || rects.Count == 0 || dividions.Count != rects.Count)
            {
                return GenerateMap(5, map);
            }

            // 部屋と通路を繋げる
            CreateConnection(map, dividions, rects);

            // 通路を作る
            //foreach (var divide in dividions)
            //{
              //  for(var i = 0; i < divide.Length; i++)
                //{
                 //  map[divide[i].y - 1, divide[i].x - 1] = 1;
                //}
            //}

            // 周りを壁にする
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    if (i == 0 || i == map.GetLength(0) - 1 || j == 0 || j == map.GetLength(1) - 1)
                        map[i, j] = 0;
                }
            }

            return map;
        }

        /// <summary>
        /// 部屋同士を接続する
        /// </summary>
        /// <param name="map">int型の二次元配列</param>
        /// <param name="dividions">通路</param>
        /// <param name="rects">部屋</param>
        private static void CreateConnection(int[,] map,List<Point[]> dividions,List<Rect> rects)
        {
            for (var i = 0; i < dividions.Count - 1; i++)
            {
                var bRect = rects[i];

                var nRect = rects[i + 1];

                var divide = dividions[i];
                var isXDivide = IsXDivide(divide);

                Func<Rect, Point[], bool, int> getDistance = (rect, div, isX) =>
                {
                    var distance =  isX ? rect.StartPosition.x - div[0].x : rect.StartPosition.y - div[0].y;
                    return distance;
                };

                Func<Rect, bool, Point> getStartPositionOfConnection = (rect, isX) =>
                {
                    var x = !isX ? new Random().Next(rect.StartPosition.x,rect.StartPosition.x + rect.Size.x) : rect.StartPosition.x; 
                    var y = isX ? new Random().Next(rect.StartPosition.y, rect.StartPosition.y + rect.Size.y) : rect.StartPosition.y;
                    return new Point(x, y);
                };

                Action<int, bool, Point,Rect> connectionReflectMap = (distance, isX, connectionStartPoint,rect) =>
                  {
                      for (var j = 0; j < MathF.Abs(distance); j++)
                      {
                          var d = j * (int)(MathF.Abs(distance) / distance) - (distance < 0 ? 1 : 0);

                          var x = !isX ? connectionStartPoint.x : rect.StartPosition.x - d;
                          var y = isX ? connectionStartPoint.y : rect.StartPosition.y - d;

                          x = x < 0 ? 0 : x < map.GetLength(1) ? x: map.GetLength(1) - 1;
                          y = y < 0 ? 0 : y < map.GetLength(0) ? y: map.GetLength(0) - 1;

                          map[y, x] = 4 + i;
                      }
                  };

                Action<Point, Point, Point[]> roadReflectMap = (startConnectionPoint1, startConnectionPoint2, divide) =>
                    {
                        if (divide.Length == 0)
                            return;
                        if (divide.Length == 1)
                        {
                            map[divide[0].y, divide[1].x] = 1;
                            return;
                        }

                        var isX = divide[0].x == divide[1].x;

                        if (isX)
                        {
                            var divideFirstPoint = startConnectionPoint1.y < startConnectionPoint2.y ?
                            startConnectionPoint1 : startConnectionPoint2;
                            var divideEndPoint = startConnectionPoint1.y >= startConnectionPoint2.y ?
                            startConnectionPoint1 : startConnectionPoint2;

                            for (var dY = divideFirstPoint.y; dY <= divideEndPoint.y; dY++)
                            {
                                map[dY, divide[0].x] = 1;
                            }

                        }
                        else
                        {
                            var divideFirstPoint = startConnectionPoint1.x < startConnectionPoint2.x ?
                            startConnectionPoint1 : startConnectionPoint2;
                            var divideEndPoint = startConnectionPoint1.x >= startConnectionPoint2.x ?
                            startConnectionPoint1 : startConnectionPoint2;

                            for (var dX = divideFirstPoint.x; dX <= divideEndPoint.x; dX++)
                            {
                                map[divide[0].y, dX] = 1;
                            }
                        }

                    };

                var bDistance = getDistance(bRect, divide, isXDivide);
                var bStartConnectPoint = getStartPositionOfConnection(bRect, isXDivide);
                connectionReflectMap(bDistance, isXDivide, bStartConnectPoint,bRect);

                var nDistance = getDistance(nRect, divide, isXDivide);
                var nStartConnectPoint = getStartPositionOfConnection(nRect, isXDivide);
                connectionReflectMap(nDistance, isXDivide, nStartConnectPoint, nRect);

                roadReflectMap(bStartConnectPoint, nStartConnectPoint, divide);
            }

            for(var i = 0; i < rects.Count; i++)
            {
                for(var y  = rects[i].StartPosition.y; y < rects[i].StartPosition.y + rects[i].Size.y; y++)
                {
                    for (var x = rects[i].StartPosition.x; x < rects[i].StartPosition.x + rects[i].Size.x; x++)
                    {
                        map[y, x] = 2;
                    }
                }
            }
        }

        private static List<Rect> ResizeRects(List<Rect> rects)
        {
            var resizedRect = new List<Rect>();
            rects.ForEach(r => 
            {
                var resize = new Point(r.Size.x - 2, r.Size.y - 2);
                var rePosition = new Point(r.StartPosition.x + 1, r.StartPosition.y + 1);
               resizedRect.Add(new Rect(rePosition.x, rePosition.y, resize.x, resize.y));
            });

            return resizedRect;
        }

        public static void DebugConsole(int[,] map)
        {

            Console.WriteLine("");

            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine("");
            }
        }


        private static bool IsXDivide(Point[] divide)
        {
            if (divide.Length < 2)
                return false;

            return divide[0].x == divide[1].x;
        }

        public static void DebugConsole(Rect rect)
        {
            Console.WriteLine("");
            Console.WriteLine(string.Format("Rect position X:{0},Y:{1},size X:{2},Y{3}", rect.StartPosition.x, rect.StartPosition.y, rect.Size.x, rect.Size.y));
            Console.WriteLine("");
        }
    }

    public class Rect
    {
        public readonly Point StartPosition;
        public readonly Point Size;


        private static int smallestSize = 4;
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
