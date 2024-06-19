namespace Scripts.Enums
{
    public enum LayoutOrder
    {
        BackGroundTilemap,
        BaseTilemap,
        ShadowsTilemap,
        Decoration,
        SkillSearchTilemap,
        SkillActiveTilemap,
    }

    public enum LayerType
    {
        Background = 0,
        Floor = 10,
        StructureShadow = 20, // ������ �׸���
        Structure = 30, // ������
        Wall = 40, // ��
        Unit = 50, // ����
        Item = 60, // ������
        Effect = 70, // ����Ʈ
        SkillSearchRange = 80, // ��ų Ž�� ����
        SkillImage = 90, // ��ų �̹���
        SkillEffect = 100, // ��ų ����Ʈ
        UI = 110, // UI
        UIEffect = 120, // UI ����Ʈ
    }
}
