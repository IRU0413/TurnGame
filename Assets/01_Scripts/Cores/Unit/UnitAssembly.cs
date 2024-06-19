namespace Scripts.Cores.Unit
{
    public class UnitAssembly : Assembly
    {
        public UnitCore UnitCore => BaseCore as UnitCore;
    }
}
