using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Interface
{
    public interface IPathCreator
    {
        List<Vector2Int> CreatePath(Vector2Int startPos, int duration);
    }
}