using UnityEngine;

namespace Controllers
{
    [CreateAssetMenu(menuName = "Audio/Playlist")]
    public class Playlist : ScriptableObject
    {
        public AudioClip[] tracks;
    }
}
