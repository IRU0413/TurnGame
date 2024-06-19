using Scripts.Enums;
using UnityEngine;

namespace Scripts.Scene
{
    [DisallowMultipleComponent]
    public abstract class BaseScene : MonoBehaviour
    {
        public SceneType SceneType { get; protected set; } = Enums.SceneType.None;

        private void Start()
        {
            Init();
        }
        public abstract void Init();
        public virtual void Clear()
        {
            Debug.Log($"{this.GetType()}Scene Clear");
        }
    }
}
