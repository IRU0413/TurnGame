using UnityEngine;

namespace Scripts.Data
{
    public class AstarNode
    {
        public Vector2Int _coordinate;
        public float G; // 이동 비용
        public float H; // 목표 노드까지의 추정 이동 비용
        public float F => G + H; // 총 비용

        public Vector2Int Coordinate => _coordinate;
        public AstarNode BeforeNode; // 이전 노드

        public AstarNode(Vector2Int coordinate)
        {
            _coordinate = coordinate;
            Reset();
        }
        public void Reset()
        {
            SetNode(0, float.MaxValue, null);
        }
        public void SetNode(float g, float h, AstarNode beforeNode)
        {
            G = g;
            H = h;
            BeforeNode = beforeNode;
        }
    }
}