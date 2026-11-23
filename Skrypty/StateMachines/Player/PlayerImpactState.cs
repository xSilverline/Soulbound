using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerImpactState : PlayerBaseState
    {
        
        private readonly int _impactHash = Animator.StringToHash("Impact");
        private readonly int _impactBlockHash = Animator.StringToHash("BlockImpact");
        private const float AnimatorFadeTime = 0.1f;
        private float _duration = 1f;
        
        public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void Enter()
        {
            
            StateMachine.Animator.CrossFadeInFixedTime(
                StateMachine.Health.IsInvulnerable() ? _impactBlockHash : _impactHash, AnimatorFadeTime);
            
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            _duration -= deltaTime;
            if (_duration <= 0)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit()
        {
            
        }
    }
}
