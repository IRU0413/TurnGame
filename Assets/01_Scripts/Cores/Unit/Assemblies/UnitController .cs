namespace Scripts.Cores.Unit.Assemblies
{
    public class UnitController : UnitAssembly
    {
        private bool _isActive; // Ȱ��ȭ ����

        protected bool _isCancleActive; // ĵ�� ��Ȱ��ȭ

        private void Update()
        {
            // �ش� ��Ʈ�� ��ǰ �ʱ�ȭ ����
            if (!IsInitialized)
                return;

            // ��Ȱ��ȭ ��, �� �� �ִ� �͵�
            if (!_isActive)
            {
                if (StartActionPoint())
                {
                    _isActive = true;
                    return;
                }
            }
            // Ȱ��ȭ ��, �� �� �ִ� �͵�
            else
            {
                if (CancleActionPoint() && _isCancleActive)
                {
                    _isActive = false;
                    return;
                }

                if (!Action())
                {
                    _isActive = false;
                    EndActionPoint();
                }
            }
        }
        protected virtual bool CancleActionPoint()
        {
            return true;
        }

        protected virtual bool StartActionPoint()
        {
            return true;
        }
        protected virtual bool Action() { return true; }
        protected virtual bool EndActionPoint() { return true; }
    }
}