using UnityEngine;

namespace Scripts.Cores.Unit.ControlAssemblies
{
    // ���� ��Ʈ�ѷ�
    public class UnitControlAssembly : Assembly
    {
        public UnitCore Unit => Base as UnitCore;
        public Vector2Int UnitPos => new Vector2Int((int)Base.Tr.position.x, (int)Base.Tr.position.y);
    }
}
