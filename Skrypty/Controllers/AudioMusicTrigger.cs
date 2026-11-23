using UnityEngine;

namespace Controllers
{
    public class AreaMusicTrigger : MonoBehaviour
    {
        public enum AreaType { Forest, City }
        public AreaType areaType;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (areaType == AreaType.Forest)
            {
                if (!GameManager.Instance.CheckAggro())
                {
                    AudioManager.Current?.SetPlaylist(AudioManager.Current.forestPlaylist);
                }
                else
                {
                    AudioManager.Current?.PreparePlaylist(AudioManager.Current.forestPlaylist);
                }
            }


            if (areaType != AreaType.City) return;
            if (!GameManager.Instance.CheckAggro())
            {
                AudioManager.Current?.SetPlaylist(AudioManager.Current.cityPlaylist);
            }
            else
            {
                AudioManager.Current?.PreparePlaylist(AudioManager.Current.cityPlaylist);
            }

        }
    }
}