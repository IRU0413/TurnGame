using Scripts.Manager;

namespace Scripts.Scene
{
    public class DunGeonScene : BaseScene
    {
        public override void Init()
        {
            SceneType = Enums.SceneType.Dungeon;
            OnStage();
        }

        private void OnStage()
        {
            var stage = GameManager.Resource.LoadManager<StageManager>();
        }
    }
}