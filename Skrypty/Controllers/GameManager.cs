using System.Collections;
using Combat;
using InputSystem;
using SaveSystem;
using StateMachines.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public enum InputMode
    {
        UI,
        Gameplay
    }

    public enum LoadType
    {
        NewGame,
        LoadGame,
        Restart
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static LoadType PendingLoadType = LoadType.NewGame;

    
        [SerializeField] private Texture2D customCursor;
        [SerializeField] private Vector2 cursorHotspot = Vector2.zero;
        private Health _playerHealth;
        private Health _bossHealth;
        private CanvasGroup _dimmerPanel;
        private GameObject _dimmerScreen;
        private GameObject _deathScreen;
        private GameObject _winScreen;
        private GameObject _menuScreen;
        
        private int _currentEnemiesAggroed = 0;
        private const float FadeTime = 3f;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (customCursor != null)
                Cursor.SetCursor(customCursor, cursorHotspot, CursorMode.Auto);
        }
    
        public void RegisterSceneDependencies(Health health, Health bossHealth,CanvasGroup dimmer, GameObject dimmerScreenObj, GameObject deathScreenObj, GameObject menuScreenObj, GameObject winScreenObj)
        {
            ClearSceneDependencies();
            _playerHealth = health;
            _bossHealth = bossHealth;
            _dimmerPanel = dimmer;
            _dimmerScreen = dimmerScreenObj;
            _deathScreen = deathScreenObj;
            _menuScreen = menuScreenObj;
            _winScreen = winScreenObj;

            if (_playerHealth != null)
                _playerHealth.OnDie += LoadDeadScreen;
            
            if( _bossHealth != null)
                bossHealth.OnDie += LoadWinScreen;
            
            StartCoroutine(HandleSceneStart());
        }
    
        public void ClearSceneDependencies()
        {
            if (_playerHealth)
                _playerHealth.OnDie -= LoadDeadScreen;
            
            if (_bossHealth)
                _bossHealth.OnDie -= LoadWinScreen;

            _playerHealth = null;
            _bossHealth = null;
            _dimmerPanel = null;
            _deathScreen = null;
            _dimmerScreen = null;
            _winScreen = null;
            _menuScreen = null;
            
        }
    
        private void OnDestroy()
        {
            ClearSceneDependencies();
        }

        public void AddAggro()
        {
            _currentEnemiesAggroed++;
            if(_currentEnemiesAggroed == 1)
             AudioManager.Current?.EnemiesAggroed();
        }
        
        public void RemoveAggro()
        {
            _currentEnemiesAggroed = Mathf.Max(_currentEnemiesAggroed - 1, 0);
            if(_currentEnemiesAggroed == 0)
             AudioManager.Current?.NoEnemiesAggroed();
        }

        public bool CheckAggro()
        {
            return _currentEnemiesAggroed > 0;
        }

        public void ShowMenu()
        {
            SetMode(InputMode.UI);
            Time.timeScale = 0f;
            _menuScreen.SetActive(true);
        }

        public void HideMenu()
        {
            SetMode(InputMode.Gameplay);
            Time.timeScale = 1f;
            _menuScreen.SetActive(false);
        }
    
        private IEnumerator HandleSceneStart()
        {
            SetMode(InputMode.Gameplay);

            if (_dimmerScreen && _dimmerPanel)
            {
                _dimmerScreen.SetActive(true);
                _dimmerPanel.alpha = 1f;
            }
            if (PendingLoadType == LoadType.LoadGame)
            {
                var data = SaveSystem.SaveSystem.Load();
                if (data != null && SceneManager.GetActiveScene().name == data.sceneName)
                {
                    var playerGo = FindFirstObjectByType<PlayerStateMachine>();
                    if (playerGo)
                    {
                        playerGo.transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
                        playerGo.transform.rotation = new Quaternion(data.rotX, data.rotY, data.rotZ, data.rotW);
                    }
                }
            }
            if (_dimmerScreen && _dimmerPanel) 
            {  
                yield return StartCoroutine(FadeFromBlack(_dimmerPanel));
                _dimmerScreen.SetActive(false);
            }
            
            if (PendingLoadType == LoadType.NewGame)
            {
                SaveSystem.SaveSystem.Save(new SaveData
                {
                    sceneName = SceneManager.GetActiveScene().name,
                    playerX = _playerHealth.transform.position.x,
                    playerY = _playerHealth.transform.position.y,
                    playerZ = _playerHealth.transform.position.z,
                    rotX = _playerHealth.transform.rotation.x,
                    rotY = _playerHealth.transform.rotation.y,
                    rotZ = _playerHealth.transform.rotation.z,
                    rotW = _playerHealth.transform.rotation.w
                });
            }
        
        }

        public void SetMode(InputMode mode)
        {
            switch (mode)
            {
                case InputMode.Gameplay:
                    GameInput.Instance.EnablePlayer();
                    GameInput.Instance.DisableUI();
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                case InputMode.UI:
                    GameInput.Instance.EnableUI();
                    GameInput.Instance.DisablePlayer();
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    break;
            }
        }

        private void LoadDeadScreen()
        {
            StartCoroutine(LoadFinalScreenCoroutine(_deathScreen));
        }

        private void LoadWinScreen()
        {
            StartCoroutine(LoadFinalScreenCoroutine(_winScreen));
        }

        private IEnumerator LoadFinalScreenCoroutine(GameObject screen)
        {
            SetMode(InputMode.UI);
            if (_dimmerScreen && _dimmerPanel)
            {
                _dimmerScreen.SetActive(true);
                yield return StartCoroutine(FadeInRoutine(_dimmerPanel));
            }

            if (screen)
                screen.SetActive(true);

            if (_dimmerPanel)
                yield return StartCoroutine(FadeFromBlack(_dimmerPanel));

            if (_dimmerScreen)
                _dimmerScreen.SetActive(false);
        
        }
    
        private IEnumerator FadeInRoutine(CanvasGroup dimmerPanel)
        {
            float startAlpha = dimmerPanel.alpha;
            float time = 0f;
            while (time < FadeTime)
            {
                time += Time.deltaTime;
                dimmerPanel.alpha = Mathf.Lerp(0f, 1f, time / FadeTime);
                yield return null;
            }

            dimmerPanel.alpha = 1f;
        }
    
        private IEnumerator FadeFromBlack(CanvasGroup dimmerPanel)
        {
            float time = 0f;
            while (time < FadeTime)
            {
                time += Time.deltaTime;
                dimmerPanel.alpha = Mathf.Lerp(1f, 0f, time / FadeTime);
                yield return null;
            }
            dimmerPanel.alpha = 0f;
        }
    
        public void LoadLastCheckpoint()
        {
            PendingLoadType = LoadType.LoadGame;
            var data = SaveSystem.SaveSystem.Load();
            if (data == null) return;
        
            StartCoroutine(LoadSceneRoutine(data.sceneName));
        }

        public void QuitGame()
        {
            PendingLoadType = LoadType.NewGame;
            StartCoroutine(LoadSceneRoutine("Menu"));
        }

        public void RestartGame()
        {
            PendingLoadType = LoadType.Restart;
            StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().name));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            if (_dimmerScreen && _dimmerPanel)
            {
                _dimmerScreen.SetActive(true);
                yield return StartCoroutine(FadeInRoutine(_dimmerPanel));
            }
            ClearSceneDependencies();
        
            var op = SceneManager.LoadSceneAsync(sceneName);
            while (op is { isDone: false })
                yield return null;
        }



    }
}