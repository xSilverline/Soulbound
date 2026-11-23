using System.Collections;
using Combat;
using UnityEngine;

namespace StateMachines.Boss
{
    public class BossFireAttackState : BossBaseState
    {
        private readonly int _attackHash = Animator.StringToHash("SpellFront2H");
        private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
        private readonly int _speedHash = Animator.StringToHash("Speed");
        private const string AttackTag = "BossFireAttack";
        private const float AnimatorDampTime = 0.1f;
        private const float AnimatorFadeTime = 0.1f;
        private FireAttack _fireAttack;

        public BossFireAttackState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            FacePlayer();
            _fireAttack = BossStateMachine.FireAttack;
            BossStateMachine.Animator.CrossFadeInFixedTime(_locomotionHash,AnimatorFadeTime);
            BossStateMachine.StartCoroutine(AttackSequence());
        }

        public override void Tick(float deltaTime)
        {
            BossStateMachine.Animator.SetFloat(_speedHash,0,AnimatorDampTime,deltaTime);
            if (GetNormalizedTime(BossStateMachine.Animator, AttackTag) >= 1f)
            {
                BossStateMachine.SwitchState(new BossIdleState(BossStateMachine));
            }
        }
        
        private IEnumerator AttackSequence()
        {
            _fireAttack.ActivateFlame();
            yield return new WaitForSeconds(0.1f);
            BossStateMachine.Animator.CrossFadeInFixedTime(_attackHash,AnimatorFadeTime);
            yield return null;
        }
        

        public override void Exit()
        {

        }
    }
}
