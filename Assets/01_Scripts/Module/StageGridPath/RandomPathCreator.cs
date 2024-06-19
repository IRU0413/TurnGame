using Scripts.Interface;
using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Module.StageGridPath
{
    public class RandomPathCreator : IPathCreator
    {
        public List<Vector2Int> CreatePath(Vector2Int startPos, int duration)
        {
            List<Vector2Int> searchAblePosList = new List<Vector2Int>();
            List<Vector2Int> path = new List<Vector2Int>();
            Vector2Int crrPos = startPos;
            path.Add(crrPos);

            // �� ����
            for (int i = 0; i < duration; i++)
            {
                UpdateSearchablePositions(crrPos, path, searchAblePosList);
                if (searchAblePosList.Count == 0)
                    break; // �� �̻� Ȯ���� �� ���� ��� ���� ����

                Vector2Int newPos = GetRandomElement(searchAblePosList);
                path.Add(newPos);
                crrPos = newPos;
                searchAblePosList.Remove(crrPos);
            }

            return path;
        }

        // ������ ��Ҹ� �����ϴ� �Լ�
        private Vector2Int GetRandomElement(List<Vector2Int> searchAblePosList)
        {
            int index = Random.Range(0, searchAblePosList.Count);
            foreach (Vector2Int element in searchAblePosList)
            {
                if (index == 0)
                {
                    return element;
                }
                index--;
            }
            return Vector2Int.zero; // �⺻�� ��ȯ, �����δ� �������� ����
        }
        private void UpdateSearchablePositions(Vector2Int currentPosition, List<Vector2Int> pathList, List<Vector2Int> searchAblePosList)
        {
            foreach (var dir in DirectionInt2D.CardinalDirection)
            {
                Vector2Int searchAblePos = currentPosition + dir;
                if (pathList.Contains(searchAblePos) || searchAblePosList.Contains(searchAblePos))
                    continue;
                searchAblePosList.Add(searchAblePos);
            }
        }
    }
}