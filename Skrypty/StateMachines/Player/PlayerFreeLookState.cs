using InputSystem;
using TMPro.SpriteAssetUtilities;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        // private static readonly int FreeLookSpeed = Animator.StringToHash("FreeLookSpeed");
        private static readonly int FreeLookForward = Animator.StringToHash("FreeLookForward");
        private static readonly int FreeLookRight = Animator.StringToHash("FreeLookRight");
        private static readonly int FreeLookBlendTree = Animator.StringToHash("FreeLookBlendTree");
        private const float AnimatorDampTime = 0.1f;
        private Vector2 _dodgeDirectionInput;
        private float _remainingDodgeTime;

        public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.InputReader.TargetEvent += OnTarget;
            StateMachine.InputReader.DodgeEvent += OnDodge;
            StateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTree,0.1f);
        }

        public override void Tick(float deltaTime)
        {
            if (GameInput.Instance.IsUIEnabled)
            {
                StateMachine.CinemachineInputAxisController.enabled = false;
            }
            else
            {
                StateMachine.CinemachineInputAxisController.enabled = true;
            }
            
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
            
            Vector3 movement = CalculateMovement(deltaTime);
            
            Move(movement * StateMachine.FreeLookMovementSpeed,deltaTime);
            UpdateAnimator(deltaTime);
            //
            if (StateMachine.InputReader.MovementValue == Vector2.zero)
            {
                StateMachine.Animator.SetFloat(FreeLookForward,0,AnimatorDampTime,Time.deltaTime);
                StateMachine.Animator.SetFloat(FreeLookRight,0,AnimatorDampTime,Time.deltaTime);
                return;
            }
            FaceMovementDirection(deltaTime * StateMachine.RotationDamp);
            
        }

        

        public override void Exit()
        {
            
            StateMachine.Animator.SetFloat(FreeLookForward,0);
            StateMachine.Animator.SetFloat(FreeLookRight,0);
            StateMachine.InputReader.TargetEvent -= OnTarget;
            StateMachine.InputReader.DodgeEvent -= OnDodge;
        }

        private void OnTarget()
        {
            if(!StateMachine.Targeter.SelectTarget()) { return;}
            
            StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
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
        
        private void FaceMovementDirection(float deltatime)
        {
            Vector3 forward = StateMachine.MainCameraTransform.forward;
            forward.y = 0f;
            forward.Normalize();
            StateMachine.transform.rotation = Quaternion.Lerp(StateMachine.transform.rotation,Quaternion.LookRotation(forward),deltatime );
        }
        
        public void UpdateAnimator(float deltaTime)
        {
            if (StateMachine.InputReader.MovementValue.y == 0f)
            {
                StateMachine.Animator.SetFloat(FreeLookForward,0,0.1f,deltaTime);
            }
            else
            {
                float value = StateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
                StateMachine.Animator.SetFloat(FreeLookForward,value,0.1f,deltaTime);
            }
           
            if (StateMachine.InputReader.MovementValue.x == 0f)
            {
                StateMachine.Animator.SetFloat(FreeLookRight,0,0.1f,deltaTime);
            }
            else
            {
                var value = StateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
                StateMachine.Animator.SetFloat(FreeLookRight,value,0.1f,deltaTime);
            }
        }
    }
}
