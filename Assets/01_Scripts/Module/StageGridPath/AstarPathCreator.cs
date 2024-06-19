using Scripts.Interface;
using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Module.StageGridPath
{
    public class AstarPathCreator : IPathCreator
    {
        public List<Vector2Int> CreatePath(Vector2Int startPos, int duration)
        {
            List<Vector2Int> path;
            Vector2Int goalPos = Vector2Int.zero;

            int distance = Mathf.RoundToInt(Mathf.Sqrt(duration));
            bool isXPlus = Random.value < 0.5f;
            bool isYPlus = Random.value < 0.5f;
            int x = Random.Range(0, distance + 1);
            int y = distance - x;
            x = isXPlus ? x : -x;
            y = isYPlus ? y : -y;
            goalPos = new Vector2Int(Mathf.Clamp(startPos.x + x, -distance, distance), Mathf.Clamp(startPos.y + y, -distance, distance));

            if (startPos == goalPos)
                path = new List<Vector2Int>();
            else
                path = Algorithms.AStar.GetPosPath(startPos, goalPos);

            return path;
        }
    }
}