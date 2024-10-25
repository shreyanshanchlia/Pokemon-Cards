using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class LevelLoader : MonoBehaviour
    {
        public void LoadLevel(string level)
        {
            SceneManager.LoadScene(level);
        }
    }
}