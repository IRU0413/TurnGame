namespace Scripts.Enums
{
    // 룸 생성 순서
    // Safe > Boss > Elite > Event > Normal
    public enum RoomType
    {
        None = 0,
        SafeRoom, // 안전 지대

        NormalRoom, // 일반
        EliteRoom, // 엘리트
        EventRoom, // 이벤트
        BossRoom, // 보스 방

        MerchantRoom, // 상점
        SkillRoom, // 스킬
        WeaponRoom, // 무기
    }

}
