namespace Scripts.Enums
{
    public enum AssemblyStateType
    {
        None = 0,
        Ready, // Ready 키 세팅이 되었고 자신이 실행하고 싶은 상태라면 Choose 상태로 넘어갈 수 있는 상태
        Choose, // Choose 자신의 기능이 선택된 상태이며 해당 부품의 기능을 사용할 수 있게 키 세팅이 되어있는 키를 눌러 해당 부품에게 명령을 하달한다.
        Action, // OnAction 현재 주어진 명령을 수행하는 상태이며 현재 상태에서는 행동이 끝나기 전 까지 상태는 지속되며 중간에 행동이 끝켰거나 정상적으로 행동을 끝났다면 End로 변경 시켜준다,
        End,
    }
}
