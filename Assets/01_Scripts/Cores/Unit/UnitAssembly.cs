namespace Scripts.Cores.Unit
{
    // 기능 또는 모델에 가까운 녀석들.
    public class UnitAssembly : Assembly
    {
        public UnitCore Unit => Base as UnitCore;
    }
}
