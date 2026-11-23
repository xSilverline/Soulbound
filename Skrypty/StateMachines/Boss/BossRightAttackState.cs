using UnityEngine;

namespace StateMachines.Boss
{
    public class BossRightAttackState : BossBaseState
    {
        private readonly int _attackHash = Animator.StringToHash("HandAttackRight");
        private const string AttackTag = "BossRightAttack";
        private readonly int _speedHash = Animator.StringToHash("Speed");
        private const float AnimatorDampTime = 0.1f;
        private const float AnimatorFadeTime = 0.1f;

        public BossRightAttackState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            FacePlayer();
            BossStateMachine.WeaponDamageRight.SetAttack(BossStateMachine.AttackDamage,BossStateMachine.AttackKnockback);
            BossStateMachine.Animator.CrossFadeInFixedTime(_attackHash,AnimatorFadeTime);
        }

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(BossStateMachine.Animator, AttackTag) >= 1)
            {
                BossStateMachine.SwitchState(new BossIdleState(BossStateMachine));
            }
        }

        public override void Exit()
        {
            
        }
    }
}
