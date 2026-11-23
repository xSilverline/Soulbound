using System.Collections;
using Controllers;
using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyIdleState : EnemyBaseState
    {
        private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
        private readonly int _speedHash = Animator.StringToHash("Speed");
        private const float AnimatorDampTime = 0.1f;
        private const float AnimatorFadeTime = 0.1f;
        
        private Coroutine _healRoutine;
        
        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            

        }


        public override void Enter()
        {
            EnemyStateMachine.soundTimer = Random.Range(10f,15f);
            EnemyStateMachine.StatusBar.SetActive(false);
            EnemyStateMachine.Animator.CrossFadeInFixedTime(_locomotionHash,AnimatorFadeTime);
            if (EnemyStateMachine.isInCombat)
            {
                EnemyStateMachine.isInCombat = false;
                GameManager.Instance.RemoveAggro();
            }
            if (EnemyStateMachine.Health.GetCurrentHealth() < EnemyStateMachine.Health.GetMaxHealth())
            {
                _healRoutine = EnemyStateMachine.StartCoroutine(SlowHealRoutine());
            }
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            EnemyStateMachine.soundTimer -= deltaTime;
            if (EnemyStateMachine.soundTimer <= 0)
            {
                EnemyStateMachine.EnemyAudioController.PlayIdleGrawlSound();
                EnemyStateMachine.soundTimer = Random.Range(10f,15f);
            }
            
            
            if (IsInChaseRange())
            {
                EnemyStateMachine.SwitchState(new EnemyChasingState(EnemyStateMachine));
                return;
            }
            
            EnemyStateMachine.Animator.SetFloat(_speedHash,0,AnimatorDampTime,deltaTime);
        }

        private IEnumerator SlowHealRoutine()
        {
            const float healInterval = 1f;
            while (EnemyStateMachine.Health.GetCurrentHealth() < EnemyStateMachine.Health.GetMaxHealth())
            {
                EnemyStateMachine.Health.Heal(EnemyStateMachine.HealingValue);
                yield return new WaitForSeconds(healInterval);
            }
            _healRoutine = null;
        }

        public override void Exit()
        {
            if (_healRoutine == null) return;
            EnemyStateMachine.StopCoroutine(_healRoutine);
            _healRoutine = null;
        }
    }
}
