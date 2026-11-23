using Controllers;
using UnityEngine;
using SaveSystem;

namespace UI
{
    public class DeathScreenController : MonoBehaviour
    {

        public void LoadLastCheckpoint()
        {
           GameManager.Instance.LoadLastCheckpoint();
        }

        public void ResetLevel()
        {
            GameManager.Instance.RestartGame();
        }
        public void QuitGame()
        {
            GameManager.Instance.QuitGame();
        }
    }
}
