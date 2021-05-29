﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator.Code.Generator
{
    /// <summary>
    ///  ランダムなマップを生成するクラス（区域分割法）
    /// </summary>
    public class RandomMapGenerator
    {
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
            List<Rect> rects = null,
            bool? isX = null
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
            isX = isX == null ? new Random().Next(0, 2) == 0 : !isX.Value;

            //  通路を生成する初期位置をを決める
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


            if (smallRect.IsValidSize)
            {
                rects.Add(smallRect);
                dividions.Add(divide);
            }


            return GenerateMap(count, map, leargeRect, dividions, rects,isX:isX);
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

            var nextRect = new Rect(1, 1, map.GetLength(1) - 2, map.GetLength(0) - 2);
            
            var  dividions = new List<Point[]>();

            var rects = new List<Rect>();

            bool? isX = new Random().Next(0, 2) == 0;

            return GenerateMap(count, map, nextRect, dividions, rects, isX);
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
            if (rects == null || rects.Count == 0 || dividions.Count != rects.Count)
            {
                return GenerateMap(5, map);
            }

            // 部屋と通路を繋げる
            // CreateConnection(map, dividions, rects);

            // 通路を作る
            foreach (var divide in dividions)
            {
                for(var i = 0; i < divide.Length; i++)
                {
                   map[divide[i].y - 1, divide[i].x - 1] = 1;
                }
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
                    var x = isX ? new Random().Next(bRect.StartPosition.y, bRect.StartPosition.y + bRect.Size.y) : rect.StartPosition.y;
                    var y = isX ? new Random().Next(bRect.StartPosition.x, bRect.StartPosition.x + bRect.Size.x) : rect.StartPosition.x;

                    return new Point(x, y);
                };

                Action<int, bool, Point> connectionReflectMap = (distance, isX, point) =>
                  {
                      for (var j = 0; j < MathF.Abs(distance) - 1; j++)
                      {
                          var d = j * (int)(MathF.Abs(distance) / distance);
                          var x = !isXDivide ? bRect.StartPosition.x - d : point.x;
                          var y = !isXDivide ? point.y : bRect.StartPosition.y - d;
                          map[x, y] = 4;
                      }
                  };

                var bDistance = getDistance(bRect, divide, isXDivide);
                var nDistance = getDistance(nRect, divide, isXDivide);

                var bStartConnectPoint = getStartPositionOfConnection(bRect, isXDivide);
                var nStartConnectPoint = getStartPositionOfConnection(nRect, isXDivide);

                connectionReflectMap(bDistance, isXDivide, bStartConnectPoint);
                connectionReflectMap(nDistance, !isXDivide, nStartConnectPoint);
                DebugConsole(map);
            }
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
