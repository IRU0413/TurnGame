using UnityEngine;
namespace Scripts.Interface
{
    public interface IDrawTile
    {
        bool IsDrawRange { get; }
        void DrawRange(Vector2 drawPoint);
        void ClearDrawRange();
    }
}