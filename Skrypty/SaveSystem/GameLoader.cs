using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public static class GameLoader
    {
        public static void ApplyCheckpointTo(GameObject player)
        {
            var data = SaveSystem.Load();
            if (data == null) return;

            if (SceneManager.GetActiveScene().name != data.sceneName) return;

            player.transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
        }
    }
}