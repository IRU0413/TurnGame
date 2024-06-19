namespace Scripts.Enums
{
    public enum MouseInputType
    {
        None = 0,
        Click,
        Clicked, // 클릭 되었을 때,
        DoubleClicked, // 더블 클릭 되었을 때,
        Preesed, // 눌림 지속 상태,
    }

    public enum KeyboardInputType
    {
        None,
        Preesed,
        Down,
        Up,
    }
}