using UnityEngine.SceneManagement;
using UnityEngine;

namespace GameManager
{
    public class GameManagerScene
    {
        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        public int GetSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
    }
}
