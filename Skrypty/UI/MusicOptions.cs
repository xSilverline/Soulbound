using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class MusicOptions : MonoBehaviour
    {

            [Header("Mixer")]
        public AudioMixer audioMixer;
        public string masterParam = "MasterVolume";
        public string musicParam = "MusicVolume";
        public string sfxParam = "SFXVolume";
        public string ambienceParam = "AmbienceVolume";


        [Header("UI")]
        public Slider masterSlider;
        public Slider musicSlider;
        public Slider sfxSlider;
        public Slider ambienceSlider;
        public Toggle muteToggle;
        
        
        const string PREF_MASTER = "vol_master";
        const string PREF_MUSIC  = "vol_music";
        const string PREF_SFX    = "vol_sfx";
        const string PREF_AMB    = "vol_amb";
        const string PREF_MUTE   = "vol_mute";

        private void Awake()
        {
            
            InitSlider(masterSlider, PREF_MASTER, 1f, OnMasterChanged);
            InitSlider(musicSlider, PREF_MUSIC, 0.8f, OnMusicChanged);
            InitSlider(sfxSlider, PREF_SFX, 0.8f, OnSfxChanged);
            InitSlider(ambienceSlider, PREF_AMB, 0.8f, OnAmbienceChanged);

            if (!muteToggle) return;
            var mute = PlayerPrefs.GetInt(PREF_MUTE, 0) == 1;
            muteToggle.isOn = mute;

            muteToggle.onValueChanged.AddListener(OnMuteToggled);
            ApplyMute(mute);
        }

        void InitSlider(Slider s, string pref, float defaultVal, UnityEngine.Events.UnityAction<float> callback)
        {
            if (s == null) return;
            float value = PlayerPrefs.GetFloat(pref, defaultVal);
            s.minValue = 0f;
            s.maxValue = 1f;
            s.value = value;
            s.onValueChanged.AddListener(callback);
            callback.Invoke(value);
        }

        public void OnMasterChanged(float value)
        {
            float db = VolumeUtil.LinearToDecibel(value);
            
            audioMixer.SetFloat(masterParam, db);
            PlayerPrefs.SetFloat(PREF_MASTER, value);
        }

        public void OnMusicChanged(float value)
        {
            float db = VolumeUtil.LinearToDecibel(value);
            audioMixer.SetFloat(musicParam, db);
            PlayerPrefs.SetFloat(PREF_MUSIC, value);
        }

        public void OnSfxChanged(float value)
        {
            float db = VolumeUtil.LinearToDecibel(value);
            audioMixer.SetFloat(sfxParam, db);
            PlayerPrefs.SetFloat(PREF_SFX, value);
        }
        
        public void OnAmbienceChanged(float value)
        {
            float db = VolumeUtil.LinearToDecibel(value);
            audioMixer.SetFloat(ambienceParam, db);
            PlayerPrefs.SetFloat(PREF_AMB, value);
        }

        public void OnMuteToggled(bool muted)
        {
            PlayerPrefs.SetInt(PREF_MUTE, muted ? 1 : 0);
            ApplyMute(muted);
        }

        void ApplyMute(bool muted)
        {
            if (muted)
            {
                audioMixer.SetFloat(masterParam, VolumeUtil.MIN_DECIBEL);
            }
            else
            {
                float masterVal = PlayerPrefs.GetFloat(PREF_MASTER, 1f);
                audioMixer.SetFloat(masterParam, VolumeUtil.LinearToDecibel(masterVal));
            }
        }
    }
}
