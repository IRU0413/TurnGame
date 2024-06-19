using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Util
{
    public static class Direction2D
    {
        // 사방향 동서남북
        private static List<Vector2> _cardinalDirectionLIst = new List<Vector2>()
        {
            Vector2.up, // UP
            Vector2.right, // RIGHT
            Vector2.down, // DOWN
            Vector2.left // LEFT
        };

        // 사방향 대각선
        private static List<Vector2> _diagonalDirectionLIst = new List<Vector2>()
        {
            Vector2.right + Vector2.down, // RIGHT DOWN
            Vector2.left + Vector2.down, // LEFT DOWN
            Vector2.left + Vector2.up, // LEFT UP
            Vector2.right + Vector2.up // RIGHT UP
        };

        public static List<Vector2> CardinalDirection => _cardinalDirectionLIst;
        public static List<Vector2> DiagonalDirection => _diagonalDirectionLIst;

        public static Vector2 GetRandomCardinalDirection()
        {
            return _cardinalDirectionLIst[Random.Range(0, 4)];
        }
        public static Vector2 GetReversalDirection(Vector2 dir)
        {
            int arrayNum = 0;

            List<Vector2> list = _cardinalDirectionLIst.Contains(dir) ? _cardinalDirectionLIst : _diagonalDirectionLIst;

            for (int i = 0; i < list.Count; i++)
                if (dir == list[i])
                    arrayNum = i;

            arrayNum += 2;

            if (arrayNum >= list.Count)
                arrayNum -= list.Count;

            return list[arrayNum];
        }
    }

    public static class DirectionInt2D
    {
        // 사방향 동서남북
        private static List<Vector2Int> _cardinalDirectionLIst = new List<Vector2Int>()
        {
            Vector2Int.up, // UP
            Vector2Int.right, // RIGHT
            Vector2Int.down, // DOWN
            Vector2Int.left // LEFT
        };

        // 사방향 대각선
        private static List<Vector2Int> _diagonalDirectionLIst = new List<Vector2Int>()
        {
            Vector2Int.right + Vector2Int.down, // RIGHT DOWN
            Vector2Int.left + Vector2Int.down, // LEFT DOWN
            Vector2Int.left + Vector2Int.up, // LEFT UP
            Vector2Int.right + Vector2Int.up // RIGHT UP
        };

        public static List<Vector2Int> CardinalDirection => _cardinalDirectionLIst;
        public static List<Vector2Int> DiagonalDirection => _diagonalDirectionLIst;

        public static Vector2Int GetRandomCardinalDirection()
        {
            return _cardinalDirectionLIst[Random.Range(0, 4)];
        }
        public static Vector2Int GetReversalDirection(Vector2Int dir)
        {
            int arrayNum = 0;

            List<Vector2Int> list = _cardinalDirectionLIst.Contains(dir) ? _cardinalDirectionLIst : _diagonalDirectionLIst;

            for (int i = 0; i < list.Count; i++)
                if (dir == list[i])
                    arrayNum = i;

            arrayNum += 2;

            if (arrayNum >= list.Count)
                arrayNum -= list.Count;

            return list[arrayNum];
        }
    }
}
