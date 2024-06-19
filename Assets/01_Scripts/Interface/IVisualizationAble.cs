using UnityEngine;

namespace Scripts.Interface
{
    public interface IVisualizationAble
    {
        GameObject VisualGroup { get; set; }
        bool IsVisualable { get; set; }
    }
}