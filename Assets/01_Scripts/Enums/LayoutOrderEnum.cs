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
        StructureShadow = 20, // 구조물 그림자
        Structure = 30, // 구조물
        Wall = 40, // 벽
        Unit = 50, // 유닛
        Item = 60, // 아이템
        Effect = 70, // 이펙트
        SkillSearchRange = 80, // 스킬 탐색 범위
        SkillImage = 90, // 스킬 이미지
        SkillEffect = 100, // 스킬 이펙트
        UI = 110, // UI
        UIEffect = 120, // UI 이펙트
    }
}
