using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerDodgingState : PlayerBaseState
    {
        private static readonly int DodgingBlendTree = Animator.StringToHash("DodgingBlendTree");
        private static readonly int DodgeForward = Animator.StringToHash("DodgeForward");
        private static readonly int DodgeRight = Animator.StringToHash("DodgeRight");
        private const float AnimatorFadeTime = 0.1f;
        
        private Vector3 _dodgeDirectionInput;
        private float _remainingDodgeTime;
        public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)
        {
            _dodgeDirectionInput = dodgingDirectionInput;
        }

        public override void Enter()
        {
            StateMachine.Health.SetInvulnerable(true);
            _remainingDodgeTime = StateMachine.DodgeDuration;
            StateMachine.Animator.SetFloat(DodgeForward,_dodgeDirectionInput.y);
            StateMachine.Animator.SetFloat(DodgeRight,_dodgeDirectionInput.x);
            StateMachine.Animator.CrossFadeInFixedTime(DodgingBlendTree,AnimatorFadeTime);
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = new Vector3();
            float dodgeBooster = StateMachine.DodgeDistance / StateMachine.DodgeDuration;
            
            if (!StateMachine.Targeter.CurrentTarget)
            {
                Vector3 forward = StateMachine.MainCameraTransform.forward;
                Vector3 right = StateMachine.MainCameraTransform.right;
            
                forward.y = 0f;
                right.y = 0f;
            
                forward.Normalize();
                right.Normalize();
                
                movement += forward * (_dodgeDirectionInput.y * dodgeBooster);
                movement += right * (_dodgeDirectionInput.x * dodgeBooster);
            }
            else
            {
                movement += StateMachine.transform.forward * (_dodgeDirectionInput.y * dodgeBooster);
                movement += StateMachine.transform.right * (_dodgeDirectionInput.x * dodgeBooster);
            }
            
            Move(movement,deltaTime);
            FaceTarget();
            _remainingDodgeTime -= deltaTime;
            if (_remainingDodgeTime <= 0f)
            {
                if (StateMachine.Targeter.CurrentTarget)
                {
                    StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
                }
                else
                {
                    StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
                }
            }
        }
        public override void Exit()
        {
            StateMachine.Health.SetInvulnerable(false);
        }
    }
}
