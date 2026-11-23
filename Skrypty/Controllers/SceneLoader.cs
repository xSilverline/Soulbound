using Combat;
using UnityEngine;

namespace Controllers
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Health playerHealth;
        [SerializeField] private CanvasGroup dimmerPanel;
        [SerializeField] private GameObject dimmerScreen;
        [SerializeField] private GameObject deathScreen;
        [SerializeField] private GameObject menuScreen;
        [SerializeField] private Health bossHealth;
        [SerializeField] private GameObject winScreen;

        private void Start()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager not found. Ensure Bootstrap instantiates it.");
                return;
            }
        
            GameManager.Instance.ClearSceneDependencies();
            GameManager.Instance.RegisterSceneDependencies(playerHealth, bossHealth,dimmerPanel, dimmerScreen, deathScreen,menuScreen,winScreen);
            GameManager.Instance.SetMode(InputMode.Gameplay);
        }
    }
}
