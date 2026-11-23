using UnityEngine;

namespace Interactions
{
    public interface IInteractable
    {
        Transform Transform { get; }
        string DisplayName { get; }
        bool CanInteract();
        void Interact();
        void OnFocus();
        void OnUnfocus();
    }
}
