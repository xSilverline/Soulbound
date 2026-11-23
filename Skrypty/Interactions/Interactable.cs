using UnityEngine;
using UnityEngine.Events;

namespace Interactions
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string displayName;
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private UnityEvent onInteract;
        [SerializeField] private GameObject interactionMessage;

        public Transform Transform { get; }
        public string DisplayName => displayName;
        public bool CanInteract() => isEnabled;



        public void Interact()
        {
            interactionMessage.SetActive(false);
            onInteract?.Invoke();
        }

        public void OnFocus()
        {
            interactionMessage.SetActive(true);
        }

        public void OnUnfocus()
        {
            interactionMessage.SetActive(false);
        }
    }
}
