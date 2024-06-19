namespace Scripts.SO
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "RoomData_Default", menuName = "ScriptableObjects/Room/Data")]
    public class RoomDataSO : ScriptableObject
    {
        [Header("= 规 积己 包访 =")]
        [SerializeField][Range(0, 100)] private int _constantCount;
        [SerializeField][Range(0, 100)] private int _maxCount;
        [SerializeField][Range(0, 100)] private float _createPercent;

        public int ConstantCount => _constantCount;
        public int MaxCount => _maxCount;
        public float CreatePercent => _createPercent;
    }
}