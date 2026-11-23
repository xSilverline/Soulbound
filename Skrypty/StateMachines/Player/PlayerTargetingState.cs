using Unity.VisualScripting;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerTargetingState : PlayerBaseState
    {
        private static readonly int TargetingBlendTree = Animator.StringToHash("TargetingBlendTree");
        private static readonly int TargetingForward = Animator.StringToHash("TargetingForward");
        private static readonly int TargetingRight = Animator.StringToHash("TargetingRight");
        private Vector2 _dodgeDirectionInput;
        private float _remainingDodgeTime;
        public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.InputReader.CancelEvent += OnCancel;
            StateMachine.InputReader.DodgeEvent += OnDodge;
            StateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTree,0.1f);
        }

        public override void Tick(float deltaTime)
        {
            if (StateMachine.InputReader.IsAttacking)
            {
                StateMachine.SwitchState(new PlayerAttackingState(StateMachine,0));
                return;
            }

            if (StateMachine.InputReader.IsBlocking)
            {
                StateMachine.SwitchState(new PlayerBlockingState(StateMachine));
                return;
            }
            if (!StateMachine.Targeter.CurrentTarget)
            {
                StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
                return;
            }
            Vector3 movement = CalculateMovement(deltaTime);
            Move(movement * StateMachine.TargetingMovementSpeed,deltaTime);

            UpdateAnimator(deltaTime);
            
            FaceTarget();
            
            
        }

        public override void Exit()
        {
            StateMachine.Animator.SetFloat(TargetingForward,0);
            StateMachine.Animator.SetFloat(TargetingRight,0);
            StateMachine.InputReader.CancelEvent -= OnCancel;
            StateMachine.InputReader.DodgeEvent -= OnDodge;
            
        }
        private void OnCancel()
        {
            StateMachine.Targeter.Cancel();
            
            StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
        }

        private void OnDodge()
        {
            if(StateMachine.InputReader.MovementValue == Vector2.zero) {return;}
            StateMachine.SwitchState(new PlayerDodgingState(StateMachine, StateMachine.InputReader.MovementValue));

        }
        
        private Vector3 CalculateMovement(float deltaTime)
        {
            Vector3 movement = new Vector3();
            
            movement += StateMachine.transform.right * StateMachine.InputReader.MovementValue.x;
            movement += StateMachine.transform.forward * StateMachine.InputReader.MovementValue.y;
                
            return movement;
            
        }

        public void UpdateAnimator(float deltaTime)
        {
            if (StateMachine.InputReader.MovementValue.y == 0f)
            {
                StateMachine.Animator.SetFloat(TargetingForward,0,0.1f,deltaTime);
            }
            else
            {
                float value = StateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
                StateMachine.Animator.SetFloat(TargetingForward,value,0.1f,deltaTime);
            }
           
            if (StateMachine.InputReader.MovementValue.x == 0f)
            {
                StateMachine.Animator.SetFloat(TargetingRight,0,0.1f,deltaTime);
            }
            else
            {
                var value = StateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
                StateMachine.Animator.SetFloat(TargetingRight,value,0.1f,deltaTime);
            }
        }
    }
}
