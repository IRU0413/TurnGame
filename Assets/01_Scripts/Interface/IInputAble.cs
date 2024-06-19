using Scripts.Enums;
using UnityEngine;

namespace Scripts.Interface
{
    public interface IInputAble
    {
        void MouseAction(MouseInputType inputType, Vector3 mousePos);
        void KeyBoardAction(KeyboardInputType inputType);
    }
}