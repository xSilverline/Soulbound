using UnityEngine;


namespace StateMachines.Enemy
{
    public class EnemyImpactState : EnemyBaseState
    {
        
        private readonly int _impactHash = Animator.StringToHash("Impact");
        private const float AnimatorFadeTime = 0.1f;
        private float _duration = 1f;
        
        public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            
        }

        public override void Enter()
        {
            EnemyStateMachine.Animator.CrossFadeInFixedTime(_impactHash,AnimatorFadeTime);
            EnemyStateMachine.EnemyAudioController.PlayImpactSound();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            
            _duration -= deltaTime;
            if (_duration <= 0)
            {
                EnemyStateMachine.SwitchState(new EnemyChasingState(EnemyStateMachine));
            }
        }

        public override void Exit()
        {
           
        }
    }
}
