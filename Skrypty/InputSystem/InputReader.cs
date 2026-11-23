using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
        public Vector2 MovementValue { get; private set; }
    
        public event Action JumpEvent;
        public event Action DodgeEvent;
        public event Action TargetEvent;
        public event Action CancelEvent;
        public event Action InteractEvent;
    
        public bool IsAttacking { get; private set; }
        public bool IsBlocking { get; private set; }
        private Controls _controls;
  
        private void Start()
        {
            _controls = GameInput.Instance.Controls;
            _controls.Player.SetCallbacks(this);
        }

        private void OnDestroy()
        {
            _controls.Player.Disable();
        }


        public void OnJump(InputAction.CallbackContext context)
        {  
            if (!context.performed)
                return;
        
            JumpEvent?.Invoke();
        }
    
        public void OnDodge(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
        
            DodgeEvent?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
        
        }

        public void OnTarget(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
        
            TargetEvent?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
        
            CancelEvent?.Invoke();
        }

        public void OnLightAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsAttacking = true;
            }
            else if (context.canceled)
            {
                IsAttacking = false;
            }
        }

        public void OnHeavyAttack(InputAction.CallbackContext context)
        {
            // throw new NotImplementedException();
        }

        public void OnBlock(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsBlocking = true;
            }
            else if (context.canceled)
            {
                IsBlocking = false;
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            // throw new NotImplementedException();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            // throw new NotImplementedException();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
        
            InteractEvent?.Invoke();
        }
    }
}
