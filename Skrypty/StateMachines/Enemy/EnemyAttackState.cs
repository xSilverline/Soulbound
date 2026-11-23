using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly int _attackHash = Animator.StringToHash("Attack1");
        private const string AttackTag = "EnemyAttack";
        private readonly int _speedHash = Animator.StringToHash("Speed");
        private const float AnimatorDampTime = 0.1f;
        private const float AnimatorFadeTime = 0.1f;
  
        public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            
        }


        public override void Enter()
        {
            FacePlayer();
            EnemyStateMachine.WeaponDamageLeft.SetAttack(EnemyStateMachine.AttackDamage, EnemyStateMachine.AttackKnockback);
            EnemyStateMachine.WeaponDamageRight.SetAttack(EnemyStateMachine.AttackDamage, EnemyStateMachine.AttackKnockback);
            EnemyStateMachine.Animator.CrossFadeInFixedTime(_attackHash,AnimatorFadeTime);
        }

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(EnemyStateMachine.Animator,AttackTag) >= 1)
            {
                EnemyStateMachine.SwitchState(new EnemyChasingState(EnemyStateMachine));
            }
        }

        public override void Exit()
        {
        
        }
    }
}
