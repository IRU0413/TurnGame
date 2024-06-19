using Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Interface
{
    public interface IRangeDrawAble
    {
        bool IsDrawAble { get; }
        DrawLayerType DrawLayer { get; }
        List<Vector2> DrawRangeList { get; }
    }
}