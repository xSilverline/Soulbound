using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerBlockingState : PlayerBaseState
    {
    
        private readonly int _blockHash = Animator.StringToHash("Block");
        private readonly int _blockOffHash = Animator.StringToHash("OffBlock");
        private const float AnimatorFadeTime = 0.1f;
    
        public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.Health.SetInvulnerable(true);
            StateMachine.Animator.CrossFadeInFixedTime(_blockHash,AnimatorFadeTime);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            if (StateMachine.InputReader.IsBlocking)
            {
                return;
            }
            if (StateMachine.Targeter.CurrentTarget)
            {
                StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
            }
            else
            {
                StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
            }

        }

        public override void Exit()
        {
            StateMachine.Animator.CrossFadeInFixedTime(_blockOffHash,AnimatorFadeTime);
            StateMachine.Health.SetInvulnerable(false);
        }
    }
}
