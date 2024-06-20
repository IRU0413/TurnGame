using UnityEngine;

namespace Scripts.Cores.Unit
{
    public class UnitAssembly : Assembly
    {
        public UnitCore UnitCore => BaseCore as UnitCore;
        public Vector2Int UnitPos => new Vector2Int((int)BaseCore.Tr.position.x, (int)BaseCore.Tr.position.y);
    }
}
