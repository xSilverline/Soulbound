using System;

namespace SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public string sceneName;
        public float playerX;
        public float playerY;
        public float playerZ;
        
        public float rotX;
        public float rotY;
        public float rotZ;
        public float rotW;
    }
}
