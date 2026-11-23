using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Controllers;
using InputSystem;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Levels To Load")]
        public string newGameLevel;
        private string _levelToLoad;
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private GameObject noSavedGameDialog;
        [SerializeField] private GameObject dimmerMenu;
        [SerializeField] private GameObject dimmerLoading;
        [SerializeField] private CanvasGroup dimmerMenuPanel;
        [SerializeField] private CanvasGroup dimmerLoadingPanel;
        [SerializeField] private float fadeTime = 3f;
        [SerializeField] private Slider progressBar;
        [SerializeField] private CanvasGroup loadingText;
        [SerializeField] private CanvasGroup loadedText;
        [SerializeField] private float blinkInterval = 1f;
        [SerializeField] private UIInputReader inputReader;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Texture2D customCursor;
        [SerializeField] private Vector2 cursorHotspot = Vector2.zero;

        private bool _inputReceived = false;
        private void Start()
        {
            GameInput.Instance.EnableUI();
            GameInput.Instance.DisablePlayer();
            
            if(inputReader != null)
                inputReader.AnyButtonEvent += OnAnyButton;
            else
                Debug.LogError("UIInputReader nie przypisany!");
            
            if (customCursor != null)
                Cursor.SetCursor(customCursor, cursorHotspot, CursorMode.Auto);
            GetVolume();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            
        }
        
        private void OnAnyButton()
        {
            _inputReceived = true;
        }

        private void GetVolume()
        {
            const string PREF_MASTER = "vol_master";
            const string PREF_MUSIC  = "vol_music";
            const string PREF_SFX    = "vol_sfx";
            const string PREF_AMB    = "vol_amb";
            const string PREF_MUTE   = "vol_mute";
            
            var masterParam = "MasterVolume";
            var musicParam = "MusicVolume";
            var sfxParam = "SFXVolume";
            var ambienceParam = "AmbienceVolume";
            
            var master = PlayerPrefs.GetFloat(PREF_MASTER, 1f);
            var music = PlayerPrefs.GetFloat(PREF_MUSIC, 1f);
            var sfx = PlayerPrefs.GetFloat(PREF_SFX, 1f);
            var ambience = PlayerPrefs.GetFloat(PREF_AMB, 1f);
            
            SetSoundVolume(master, masterParam);
            SetSoundVolume(music, musicParam);
            SetSoundVolume(sfx, sfxParam);
            SetSoundVolume(ambience, ambienceParam);
        }

        private void SetSoundVolume(float volume, string nameParam)
        {
            var db = VolumeUtil.LinearToDecibel(volume);
            audioMixer.SetFloat(nameParam, db);
        }


        public void NewGameDialogYes()
        {
            GameManager.PendingLoadType = LoadType.NewGame;
            StartCoroutine(LoadSceneRoutine(newGameLevel));
        }
        
        public void LoadGameDialogYes()
        {
            if (SaveSystem.SaveSystem.SaveExists())
            {
                GameManager.PendingLoadType = LoadType.LoadGame;
                var data = SaveSystem.SaveSystem.Load();
                StartCoroutine(LoadSceneRoutine(data.sceneName));
            }
            else
            {
                noSavedGameDialog.SetActive(true);
            }
        }
        
        public void QuitGameDialogYes()
        {
            Application.Quit();
        }

        private IEnumerator FadeInRoutine(CanvasGroup dimmerPanel)
        {
            float startAlpha = dimmerPanel.alpha;
            float time = 0f;
            while (time < fadeTime)
            {
                time += Time.deltaTime;
                dimmerPanel.alpha = Mathf.Lerp(0f, 1f, time / fadeTime);
                yield return null;
            }

            dimmerPanel.alpha = 1f;
        }
        
        private IEnumerator FadeFromBlack(CanvasGroup dimmerPanel)
        {
            float time = 0f;
            while (time < fadeTime)
            {
                time += Time.deltaTime;
                dimmerPanel.alpha = Mathf.Lerp(1f, 0f, time / fadeTime);
                yield return null;
            }
            dimmerPanel.alpha = 0f;
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            dimmerMenu.SetActive(true);
            yield return StartCoroutine(FadeInRoutine(dimmerMenuPanel));
            loadingScreen.SetActive(true);
            yield return StartCoroutine(FadeFromBlack(dimmerLoadingPanel));
            dimmerLoading.SetActive(false);
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName);
 
            loadingOperation.allowSceneActivation = false;

            while (loadingOperation.progress < 0.9f)
            {
                progressBar.value = Mathf.Clamp01(loadingOperation.progress / 1f);
                yield return null;
            }
           
            progressBar.value = 1f;

            yield return StartCoroutine(FadeFromBlack(loadingText));
            Coroutine blinking = StartCoroutine(BlinkTextRoutine(loadedText));

            while (!_inputReceived)
                yield return null;
  
            StopCoroutine(blinking);
            Cursor.lockState = CursorLockMode.Locked;
            dimmerLoading.SetActive(true);
            yield return StartCoroutine(FadeScene(audioSource,dimmerLoadingPanel, loadingOperation));
            
            
        }

        private IEnumerator FadeScene(AudioSource audioToFade, CanvasGroup dimmerPanel, AsyncOperation loadingOperation)
        {
            float time = 0f;
            float startVolume = audioToFade.volume;
            while (time < fadeTime)
            {
                time += Time.deltaTime;
                audioToFade.volume = Mathf.Lerp(startVolume, 0f, time / fadeTime);
                dimmerPanel.alpha = Mathf.Lerp(0f, 1f, time / fadeTime);
                yield return null;
            }
            // Debug.Log("Fade Complete");
            audioToFade.volume = 0f;
            dimmerPanel.alpha = 1f;
            yield return null;
            loadingOperation.allowSceneActivation = true;
        }
        
        private IEnumerator BlinkTextRoutine(CanvasGroup group)
        {
            float alpha = 0f;
            bool increasing = true;

            while (true)
            {
                if (increasing)
                {
                    alpha += Time.deltaTime * blinkInterval;
                    if (alpha >= 1f)
                    {
                        alpha = 1f;
                        increasing = false;
                    }
                }
                else
                {
                    alpha -= Time.deltaTime * blinkInterval;
                    if (alpha <= 0f)
                    {
                        alpha = 0f;
                        increasing = true;
                    }
                }

                group.alpha = alpha;
                yield return null;
            }
        }
    
    }
}
