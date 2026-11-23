using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class AudioManager : MonoBehaviour
    {
        
        public static AudioManager Current { get; private set; }
        [Header("Playlists")]
        public Playlist forestPlaylist;
        public Playlist cityPlaylist;
        public Playlist combatPlaylist;
        public Playlist deathPlaylist;
        public Playlist bossPlaylist;
        public Playlist bossDeathPlaylist;

        [Header("Crossfade")]
        public float fadeTime = 2f;
        public float volume = 0.5f;
        [Header("Audio Sources")]
        public AudioSource sourceA;
        public AudioSource sourceB;
        
        private bool _usingA = true;

        private Playlist _currentPlaylist;
        private int _currentTrackIndex;
        private Playlist _previousPlaylist;

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            SetPlaylist(forestPlaylist);
        }

        public void SetPlaylist(Playlist playlist)
        {
            if (!playlist) return;
            if (playlist == _currentPlaylist) return;
            _currentPlaylist = playlist;
            _currentTrackIndex = _currentPlaylist != bossPlaylist ? Random.Range(0, _currentPlaylist.tracks.Length-1) : 0;
            StartCoroutine(CrossfadeToTrack(_currentPlaylist.tracks[_currentTrackIndex]));
        }

        public void EnemiesAggroed()
        {
            _previousPlaylist = _currentPlaylist;
            SetPlaylist(combatPlaylist);
        }

        public void NoEnemiesAggroed()
        {
            SetPlaylist(_previousPlaylist);
        }

        public void Death()
        {
            SetPlaylist(deathPlaylist);
        }

        public void BossDeath()
        {
            SetPlaylist(bossDeathPlaylist);
        }

        public void PreparePlaylist(Playlist playlist)
        {
            _previousPlaylist = playlist;
        }

        private IEnumerator CrossfadeToTrack(AudioClip newTrack)
        {
            AudioSource active = _usingA ? sourceA : sourceB;
            AudioSource incoming = _usingA ? sourceB : sourceA;
            incoming.clip = newTrack;
            incoming.volume = 0;
            incoming.Play();
            float t = 0;

            while (t < fadeTime)
            {
                t += Time.deltaTime;
                float k = t / fadeTime;

                incoming.volume = Mathf.Lerp(0, volume, k);
                active.volume = Mathf.Lerp(volume, 0, k);

                yield return null;
            }
            active.Stop();
            _usingA = !_usingA;
            StartCoroutine(WaitForTrackEnd());
        }
        

        private IEnumerator WaitForTrackEnd()
        {
            AudioSource active = _usingA ? sourceA : sourceB;
            yield return new WaitForSeconds(active.clip.length);
            _currentTrackIndex = (_currentTrackIndex + 1) % _currentPlaylist.tracks.Length;
            StartCoroutine(CrossfadeToTrack(_currentPlaylist.tracks[_currentTrackIndex]));
        }
    }
}
