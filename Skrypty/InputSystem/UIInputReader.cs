using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class UIInputReader : MonoBehaviour, Controls.IUIActions
    {
        private Controls _controls;
    
        public event Action AnyButtonEvent;
        public event Action ConfirmEvent;
        public event Action ReturnEvent;
    
        private void Start()
        {
            _controls = GameInput.Instance.Controls;
            _controls.UI.SetCallbacks(this);
        }
    
        private void OnDestroy()
        {
            _controls?.UI.Disable();
        }

        private void OnDisable()
        {
            _controls?.UI.Disable();
        }

        public void OnNavigation(InputAction.CallbackContext context)
        {
        
        }

        public void OnAnyButton(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
        
            AnyButtonEvent?.Invoke();
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            ConfirmEvent?.Invoke();
        }

        public void OnReturn(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            ReturnEvent?.Invoke();
        }
    }
}
