using Scripts.Enums;
using Scripts.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Manager
{
    public class SceneManagerEx
    {
        public BaseScene CurrentScene => GameObject.FindObjectOfType<BaseScene>();
        public SceneType BeforeSceneType { get; private set; }

        public void LoadScene(SceneType type)
        {
            BaseScene currentScene = CurrentScene;

            currentScene.Clear();
            BeforeSceneType = currentScene.SceneType;

            SceneManager.LoadScene(GetSceneName(type));
        }
        private string GetSceneName(SceneType type)
        {
            string name = System.Enum.GetName(typeof(SceneType), type) + "Scene";
            return name;
        }
    }
}