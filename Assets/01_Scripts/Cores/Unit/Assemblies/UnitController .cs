namespace Scripts.Cores.Unit.Assemblies
{
    public class UnitController : UnitAssembly
    {
        private bool _isActive; // 활성화 여부

        protected bool _isCancleActive; // 캔슬 비활성화

        private void Update()
        {
            // 해당 컨트롤 부품 초기화 여부
            if (!IsInitialized)
                return;

            // 비활성화 시, 할 수 있는 것들
            if (!_isActive)
            {
                if (StartActionPoint())
                {
                    _isActive = true;
                    return;
                }
            }
            // 활성화 시, 할 수 있는 것들
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