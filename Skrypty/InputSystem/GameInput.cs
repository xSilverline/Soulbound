using UnityEngine;

namespace InputSystem
{
    public class GameInput : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void CreateInputSystem()
        {
            if (Instance == null)
            {
                var go = new GameObject("GameInput");
                go.AddComponent<GameInput>();
            }
        }
        public static GameInput Instance { get; private set; }
        public bool IsUIEnabled => Controls.UI.enabled;
        public Controls Controls { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Controls = new Controls();
        }

        public void EnableUI()
        {
            Controls.UI.Enable();
        }

        public void DisableUI() => Controls.UI.Disable();

        public void EnablePlayer() => Controls.Player.Enable();
        public void DisablePlayer() => Controls.Player.Disable();
    }
}