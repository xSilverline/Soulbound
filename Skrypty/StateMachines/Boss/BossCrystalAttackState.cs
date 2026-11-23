using System.Collections;
using Combat;
using UnityEngine;

namespace StateMachines.Boss
{
    public class BossCrystalAttackState : BossBaseState
    {
        private readonly int _attackHash = Animator.StringToHash("SpellCast2H");
        private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
        private readonly int _speedHash = Animator.StringToHash("Speed");
        private const float AnimatorDampTime = 0.1f;
        private const float AnimatorFadeTime = 0.1f;
        private int _attacksDone;
        private CrystalAttack _crystalAttack;
        private bool _isSkillRunning;
        public BossCrystalAttackState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void Enter()
        {
            _attacksDone = 0;
            _crystalAttack = BossStateMachine.CrystalAttack;
            _crystalAttack.OnCrystalAttackFinished += HandleSkillFinished;
            BossStateMachine.Animator.CrossFadeInFixedTime(_locomotionHash,AnimatorFadeTime);
            
            BossStateMachine.StartCoroutine(AttackSequence());
        }

        public override void Tick(float deltaTime)
        {
            BossStateMachine.Animator.SetFloat(_speedHash, 0f, AnimatorDampTime, Time.fixedDeltaTime);
        }

        public override void Exit()
        {
            _crystalAttack.OnCrystalAttackFinished -= HandleSkillFinished;
        }

        private IEnumerator AttackSequence()
        {
            while (_attacksDone < 3)
            {
                _isSkillRunning = true;
                _crystalAttack.SpawnCrystal();
                yield return new WaitForSeconds(0.1f);
                BossStateMachine.Animator.CrossFadeInFixedTime(_attackHash,AnimatorFadeTime);
                yield return new WaitUntil(() => !_isSkillRunning);
            }
            
            BossStateMachine.SwitchState(new BossIdleState(BossStateMachine));
        }
        private void HandleSkillFinished()
        {
            _isSkillRunning = false;
            _attacksDone++;
        }
    }
}
