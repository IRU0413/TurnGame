namespace Scripts.Enums
{
    public enum AssemblyStateType
    {
        None = 0,
        Ready, // Ready Ű ������ �Ǿ��� �ڽ��� �����ϰ� ���� ���¶�� Choose ���·� �Ѿ �� �ִ� ����
        Choose, // Choose �ڽ��� ����� ���õ� �����̸� �ش� ��ǰ�� ����� ����� �� �ְ� Ű ������ �Ǿ��ִ� Ű�� ���� �ش� ��ǰ���� ������ �ϴ��Ѵ�.
        Action, // OnAction ���� �־��� ������ �����ϴ� �����̸� ���� ���¿����� �ൿ�� ������ �� ���� ���´� ���ӵǸ� �߰��� �ൿ�� ���װų� ���������� �ൿ�� �����ٸ� End�� ���� �����ش�,
        End,
    }
}