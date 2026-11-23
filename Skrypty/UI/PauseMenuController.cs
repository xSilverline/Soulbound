using System;
using Controllers;
using InputSystem;
using UnityEngine;

namespace UI
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private UIInputReader inputReader;
        private void OnEnable()
        {
            inputReader.ReturnEvent += EndChat;
        }
        private void OnDisable()
        {
            inputReader.ReturnEvent -= EndChat;
        }

        public void EndChat()
        {
            GameManager.Instance.HideMenu();
        }
        
        public void LoadLastCheckpoint()
        {
            Time.timeScale = 1f;
            GameManager.Instance.LoadLastCheckpoint();
        }

        public void ResetLevel()
        {
            Time.timeScale = 1f;
            GameManager.Instance.RestartGame();
        }
        public void QuitGame()
        {
            Time.timeScale = 1f;
            GameManager.Instance.QuitGame();
        }
    }
}
