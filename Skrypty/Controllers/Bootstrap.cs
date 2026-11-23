using InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private GameObject gameManagerPrefab;
        [SerializeField] private GameObject gameInputPrefab;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (GameInput.Instance == null)
                Instantiate(gameInputPrefab);
        
            if (GameManager.Instance == null)
            {
                Instantiate(gameManagerPrefab);
            }
            SceneManager.LoadScene("Menu");
        }
    
    }
}
