using Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactions
{
    public class NpcInteract : MonoBehaviour
    {
        [SerializeField] private GameObject chatBox;

        
        public void OpenChat()
        {
            GameManager.Instance.SetMode(InputMode.UI);
            chatBox.SetActive(true);
            
        }

    }
}
