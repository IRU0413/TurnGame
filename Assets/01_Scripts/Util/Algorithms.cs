using Scripts.Data;
using Scripts.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Util
{
    public static class Algorithms
    {
        public static class AStar
        {
            public static List<Vector2Int> GetPosPath(Vector2Int start, Vector2Int goal)
            {
                Dictionary<Vector2Int, AstarNode> astartMap = new();

                Vector2Int minPoint = Vector2Int.FloorToInt(start);
                Vector2Int maxPoint = Vector2Int.FloorToInt(goal);

                if (minPoint.x > maxPoint.x)
                {
                    int temp = minPoint.x;
                    minPoint.x = maxPoint.x;
                    maxPoint.x = temp;
                }
                if (minPoint.y > maxPoint.y)
                {
                    int temp = minPoint.y;
                    minPoint.y = maxPoint.y;
                    maxPoint.y = temp;
                }

                for (int y = minPoint.y; y <= maxPoint.y; y++)
                {
                    for (int x = minPoint.x; x <= maxPoint.x; x++)
                    {
                        Vector2Int pos = new Vector2Int(x, y);
                        AstarNode node = new AstarNode(pos);

                        astartMap.Add(node.Coordinate, node);
                    }
                }

                if (!astartMap.ContainsKey(start) || !astartMap.ContainsKey(goal))
                {
                    return new List<Vector2Int>(); // 경로를 찾을 수 없는 경우 빈 리스트 반환
                }

                List<AstarNode> road = GetAStarPath(astartMap, astartMap[start], astartMap[goal]);
                return road?.ConvertAll(x => x.Coordinate) ?? new List<Vector2Int>();
            }

            public static List<AstarNode> GetAStarPath(Dictionary<Vector2Int, AstarNode> map, AstarNode start, AstarNode goal)
            {
                if (start.Coordinate == goal.Coordinate) return null;

                var openList = new List<AstarNode> { start };
                var closedList = new List<AstarNode>();

                start.G = 0;
                start.H = CalculateHScore(start.Coordinate, goal.Coordinate);

                while (openList.Count > 0)
                {
                    var currentNode = GetLowestScoreNode(openList);
                    openList.Remove(currentNode);
                    closedList.Add(currentNode);

                    if (currentNode.Coordinate == goal.Coordinate)
                    {
                        return ReconstructPath(currentNode);
                    }

                    List<AstarNode> roundNodeList = GetSurroundingNodes(map, currentNode);

                    if (roundNodeList.Count <= 0) continue;
                    foreach (var node in roundNodeList)
                    {
                        if (closedList.Contains(node)) continue;

                        float tentativeGScore = currentNode.G + 1; // 이 예제에서는 모든 이동 비용을 1로 가정

                        // 위에서 Map에 없는 것을 알아서 걸러오기 때문에 없다면 추가해주는 게 맞음
                        if (!openList.Contains(node))
                            openList.Add(node);
                        else if (tentativeGScore >= node.G)
                            continue;

                        node.SetNode(tentativeGScore, CalculateHScore(node.Coordinate, goal.Coordinate), currentNode);
                    }
                }

                return null; // 경로를 찾을 수 없음
            }

            private static List<AstarNode> ReconstructPath(AstarNode currentNode)
            {
                var path = new List<AstarNode>();
                while (currentNode != null)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.BeforeNode;
                }
                path.Reverse();
                return path;
            }

            private static List<AstarNode> GetSurroundingNodes(Dictionary<Vector2Int, AstarNode> map, AstarNode node)
            {
                var resultLIst = new List<AstarNode>();
                foreach (var dir in Direction2D.CardinalDirection)
                {
                    Vector2Int neighborPos = node.Coordinate + Vector2Int.FloorToInt(dir);
                    if (map.TryGetValue(neighborPos, out var neighbor))
                        resultLIst.Add(neighbor);
                }
                return resultLIst;
            }

            private static AstarNode GetLowestScoreNode(List<AstarNode> nodes)
            {
                AstarNode lowestNode = nodes[0];
                foreach (var node in nodes)
                {
                    if (node.F < lowestNode.F || (node.F == lowestNode.F && node.G < lowestNode.G))
                    {
                        lowestNode = node;
                    }
                }
                return lowestNode;
            }

            private static int CalculateHScore(Vector2 from, Vector2 to)
            {
                return Mathf.Abs((int)(to.x - from.x)) + Mathf.Abs((int)(to.y - from.y));
            }
        }

        public static List<Vector2> GetRangePos(SearchFormatType format, int radius, bool isOnlyRound = false)
        {
            switch (format)
            {
                case SearchFormatType.Squr:
                    return GetSquarePos(Vector2.zero, radius, isOnlyRound);
                case SearchFormatType.Rhombus:
                    return GetRhombusPos(Vector2.zero, radius, isOnlyRound);
                case SearchFormatType.Circle:
                    return GetCirclePos(Vector2.zero, radius, isOnlyRound);
                default:
                    return new();
            }
        }

        public static HashSet<Vector2> SimpleRandomWalk(Vector2 startPosition, int walkLength)
        {
            HashSet<Vector2> path = new HashSet<Vector2>();

            var previousPosition = startPosition;
            path.Add(startPosition);

            for (int i = 0; i < walkLength; i++)
            {
                var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
                path.Add(newPosition);
                previousPosition = newPosition;
            }

            return path;
        }

        /// <summary>
        /// 거리 내 임의 점까지 생성
        /// </summary>
        /// <param name="startPosition">시작 위치</param>
        /// <param name="distance">거리</param>
        /// <returns></returns>
        public static HashSet<Vector2> RandomPointInDistanceWalk(Vector2 startPosition, int distance)
        {
            HashSet<Vector2> path = new HashSet<Vector2>();
            bool[] dirCheck = new bool[4] { false, false, false, false };

            Vector2 longNodePos = startPosition;
            int longDistance = 0;
            path.Add(startPosition);

            // 원점에 제일 먼 노드에서 랜덤한 노드 선택후,
            // 그 노드에서 특정한 방향으로 노드 생성,
            // 노드를 생성하려고 하였을 떄, 생성 방향에 노드가 존재한다면 다른 방향을 체크

            while (true)
            {
                Vector2 checkNode = longNodePos;
                while (path.Contains(checkNode))
                {
                    int idx = Random.Range(0, dirCheck.Length);
                    checkNode = longNodePos + Direction2D.CardinalDirection[idx];
                }

                longNodePos = checkNode;
                longDistance = (int)Mathf.Abs(checkNode.x) + (int)Mathf.Abs(checkNode.y);

                for (int i = 0; i < path.Count; i++)
                {
                    Vector2 coordinate = path.ElementAt(i) - startPosition;
                    int checkDis = (int)Mathf.Abs(coordinate.x) + (int)Mathf.Abs(coordinate.y);

                    if (longDistance < checkDis)
                    {
                        longNodePos = path.ElementAt(i);
                        longDistance = checkDis;
                    }
                }
                if (longDistance > distance)
                    break;

                path.Add(checkNode);
            }
            return path;
        }

        public static HashSet<Vector2> LocalFrontRandomWalk(Vector2 startPosition, int walkLength)
        {
            HashSet<Vector2> path = new HashSet<Vector2>();
            bool[] dirCheck = new bool[4] { false, false, false, false };

            var previousPosition = startPosition;
            path.Add(startPosition);

            Vector2 beforeDir = default;

            while (path.Count < walkLength)
            {
                // 랜덤을 돌리지만
                int idx = Random.Range(0, 4);
                // 그 해당 방향이 체크 되어었다면
                if (dirCheck[idx] == true)
                {
                    int counting = 0;
                    for (int i = 0; i < dirCheck.Length; i++)
                    {
                        if (dirCheck[i])
                            counting++;
                    }

                    // 상하좌우 방향 막혀있는가?
                    if (counting != dirCheck.Length)
                        continue;

                    // 막혀있을 때, 원점 위치부터 제일 멀리있는 것을 구해준다.
                    float longDistance = (previousPosition - startPosition).magnitude;
                    for (int i = 0; i < path.Count; i++)
                    {
                        float checkMag = (path.ElementAt(i) - startPosition).magnitude;
                        if (longDistance < checkMag)
                        {
                            previousPosition = path.ElementAt(i);
                            longDistance = (previousPosition - startPosition).magnitude;
                        }
                    }

                    for (int i = 0; i < dirCheck.Length; i++)
                        dirCheck[i] = false;
                }

                var dir = Direction2D.CardinalDirection[idx];
                dirCheck[idx] = true;

                var newPosition = previousPosition + dir;
                if (path.Contains(newPosition))
                    continue;

                path.Add(newPosition);

                previousPosition = newPosition;
                beforeDir = dir * -1;
                for (int i = 0; i < Direction2D.CardinalDirection.Count; i++)
                {
                    if (beforeDir != Direction2D.CardinalDirection[i])
                    {
                        dirCheck[i] = false;
                        continue;
                    }

                    dirCheck[i] = true;
                }
            }

            return path;
        }

        public static HashSet<Vector2> SelectionDirectionWalk(Vector2 startPosition, int walkLength, Vector2 dir)
        {
            HashSet<Vector2> path = new HashSet<Vector2>();

            Vector2 previousPosition = startPosition; // 이전 위치
                                                      // bool isFront = false; // 앞으로 갈것인가?

            Vector2 nowdir = dir;
            Vector2 forbiddenDir = nowdir * -1; // 금지된 방향
            Vector2 beforeDir = Vector2.zero;

            // 시작 위치 넣어주기
            path.Add(previousPosition);
            previousPosition += nowdir;
            beforeDir = nowdir * -1;

            // 다음 방향 넣어주기
            path.Add(previousPosition);

            int idx = -1;
            while (path.Count < walkLength)
            {
                idx = Random.Range(0, 4);

                nowdir = Direction2D.CardinalDirection[idx];
                previousPosition += nowdir;
                beforeDir = nowdir * -1;

                path.Add(previousPosition);
            }
            return path;
        }

        public static List<Vector2> GetBonthGroupWalk()
        {
            return null;
        }

        /// <summary>
        /// 반지름에 맞는 마름모 범위 가져오기
        /// </summary>
        /// <typeparam name="T">타입</typeparam>
        /// <param name="centerNode">중앙 노드</param>
        /// <param name="map">체크해야될 맵</param>
        /// <param name="radius">반지름</param>
        /// <returns></returns>
        public static List<Vector2> GetRhombusPos(Vector2 centerNode = default, int radius = 1, bool onlyRoundNode = false)
        {
            if (radius <= 0)
                return new List<Vector2>();

            if (centerNode == default)
                centerNode = Vector2.zero;

            List<Vector2> result = new List<Vector2>();
            if (!onlyRoundNode)
                result.Add(centerNode);

            int currentRadius = (onlyRoundNode) ? radius : 1;

            for (; currentRadius <= radius; currentRadius++)
            {
                for (int directionIndex = 0; directionIndex < 4; directionIndex++)
                {
                    Vector2 searchNode = centerNode
                        + (Direction2D.CardinalDirection[directionIndex] * currentRadius);

                    result.Add(searchNode);
                    for (int diagonalStep = 1; diagonalStep < currentRadius; diagonalStep++)
                    {
                        Vector2 diagonalNode = searchNode + Direction2D.DiagonalDirection[directionIndex] * diagonalStep;
                        result.Add(diagonalNode);
                    }
                }
            }

            return result;
        }

        public static List<Vector2> GetCirclePos(Vector2 centerNode = default, int radious = 1, bool onlyRoundNode = false)
        {
            if (centerNode == default)
                centerNode = Vector2.zero;

            List<Vector2> result = new();

            List<Vector2> checkNodes = new();
            checkNodes = GetSquarePos(centerNode, radious);

            foreach (Vector2 pos in checkNodes)
            {
                float mag = (pos - Vector2.zero).magnitude;

                if (onlyRoundNode)
                {
                    if (mag < radious + 0.5f && mag >= radious)
                        result.Add(pos);
                    continue;
                }

                if (mag < radious + 0.5f)
                    result.Add(pos);
            }

            return result;
        }

        /// <summary>
        /// 반지름에 맞는 네모 범위 노드 가져오기
        /// </summary>
        /// <typeparam name="T">타입</typeparam>
        /// <param name="map">노드 맵</param>
        /// <param name="nodeXY">현재 노드</param>
        /// <param name="radius">반지름</param>
        /// <returns></returns>
        public static List<Vector2> GetSquarePos(Vector2 startPos, int radius = 1, bool onlyRoundNode = false)
        {
            if (radius <= 0)
                return null;

            List<Vector2> result = new();

            Vector2 leftDown = startPos + Direction2D.DiagonalDirection[1] * radius;
            Vector2 rightUp = startPos + Direction2D.DiagonalDirection[3] * radius;

            int minX = (int)leftDown.x;
            int minY = (int)leftDown.y;

            int maxX = (int)rightUp.x;
            int maxY = (int)rightUp.y;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (onlyRoundNode)
                    {
                        if (y <= minY || maxY <= y)
                            result.Add(new Vector2(x, y));

                        if (x <= minX || maxX <= x)
                            result.Add(new Vector2(x, y));
                        continue;
                    }

                    result.Add(new Vector2(x, y));
                }
            }

            return result;
        }


        /// <summary>
        /// 배열 범위를 벗어났는지 체크
        /// </summary>
        public static bool IsInArea<T>(T[,] map, int x, int y)
        {
            int maxX = map.GetLength(0) - 1;
            int maxY = map.GetLength(1) - 1;
            return x >= 0 && x <= maxX && y >= 0 && y <= maxY;
        }
        public static bool IsInArea<T>(T[,] map, Vector2 pos)
        {
            return IsInArea<T>(map, (int)pos.x, (int)pos.y);
        }
        public static bool IsInArea(Vector2 current, Vector2 min, Vector2 max)
        {
            return current.x >= min.x && current.x <= max.x && current.y >= min.y && current.y <= max.y;
        }
    }
}