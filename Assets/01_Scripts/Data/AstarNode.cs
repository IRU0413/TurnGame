using UnityEngine;

namespace Scripts.Data
{
    public class AstarNode
    {
        public Vector2Int _coordinate;
        public float G; // �̵� ���
        public float H; // ��ǥ �������� ���� �̵� ���
        public float F => G + H; // �� ���

        public Vector2Int Coordinate => _coordinate;
        public AstarNode BeforeNode; // ���� ���

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