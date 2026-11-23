using Combat;
using UnityEngine;

namespace SaveSystem
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private Health health;
        [SerializeField] private int healAmount = 1000;
        public void SaveToFile()
        {
            var data = new SaveData();
            data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            var pos = player.transform.position;
            data.playerX = pos.x;
            data.playerY = pos.y;
            data.playerZ = pos.z;
            
            var rot = player.transform.rotation;
            data.rotX = rot.x;
            data.rotY = rot.y;
            data.rotZ = rot.z;
            data.rotW = rot.w;
            
            health.Heal(healAmount);
            
            SaveSystem.Save(data);
            
            Debug.Log("Checkpoint activated, save done.");
        }
    }
}

