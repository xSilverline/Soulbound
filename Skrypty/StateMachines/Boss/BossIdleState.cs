using System.Collections;
using UnityEngine;

namespace StateMachines.Boss
{
    public class BossIdleState : BossBaseState
    {
        private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
        private readonly int _speedHash = Animator.StringToHash("Speed");
        private const float AnimatorDampTime = 0.1f;
        private const float AnimatorFadeTime = 0.1f;
        private float _timerBetweenAttacks = 0f;
        private bool _isThinking = false;
        private bool _waitingForAttack = false;
        private bool _skillNamePlaying = false;
        private string _errorAttack = "Error";

        public BossIdleState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            BossStateMachine.Animator.CrossFadeInFixedTime(_locomotionHash,AnimatorFadeTime);
            _timerBetweenAttacks = 1f;
            _isThinking = false;
            _waitingForAttack = false;
            _skillNamePlaying = false;

            BossStateMachine.APIBossController.AttackWordFinishedEvent += OnAttackWordReceived;
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            BossStateMachine.Animator.SetFloat(_speedHash,0,AnimatorDampTime,deltaTime);
            
            if (!_isThinking)
                DecideAndAttack();
    
            if (_waitingForAttack)
                return;
            
            _timerBetweenAttacks -= deltaTime;
            if (_timerBetweenAttacks > 0 || _skillNamePlaying) return;
            _skillNamePlaying = true;
            BossStateMachine.StartCoroutine(ShowSkillName(BossStateMachine.nextAttack));
        }

        public override void Exit()
        {
            BossStateMachine.APIBossController.AttackWordFinishedEvent -= OnAttackWordReceived;
        }

        private void DecideAndAttack()
        {
            if (_isThinking) return;
            _isThinking = true;
            _waitingForAttack = true;
            
            BossStateMachine.APIBossController.AskForAttack();
        }

        private IEnumerator ShowSkillName(int skillIndex)
        {
            if (!BossStateMachine.SkillText) yield break;

            BossStateMachine.SkillText.gameObject.SetActive(true);
            BossStateMachine.SkillText.text = BossStateMachine.attackList[skillIndex];
            yield return new WaitForSeconds(1f);
            BossStateMachine.SkillText.text = "";
            BossStateMachine.SkillText.gameObject.SetActive(false);
            
            switch (BossStateMachine.nextAttack)
            {
                case 0 or 1:
                    BossStateMachine.SwitchState(new BossChasingState(BossStateMachine));
                    yield break;
                case 2:
                    BossStateMachine.SwitchState(new BossFireAttackState(BossStateMachine));
                    yield break;
                case 3:
                    BossStateMachine.SwitchState(new BossCrystalAttackState(BossStateMachine));
                    yield break;
                default:
                    yield break;
            }
        }

        private void OnAttackWordReceived()
        {
            string attackName = BossStateMachine.APIBossController.GetAttackWord();
            int index = System.Array.IndexOf(BossStateMachine.attackList, attackName);
            BossStateMachine.nextAttack = index >= 0 ? index : Random.Range(0, 4);
            
            _waitingForAttack = false;
        }
    }
}
