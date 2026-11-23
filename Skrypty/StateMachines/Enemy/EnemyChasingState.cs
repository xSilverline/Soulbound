using Controllers;
using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyChasingState : EnemyBaseState
    {
        private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
        private readonly int _speedHash = Animator.StringToHash("Speed");
        private const float AnimatorDampTime = 0.1f;
        private const float AnimatorFadeTime = 0.1f;
  
        public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }
    
        public override void Enter()
        {
            EnemyStateMachine.soundTimer = Random.Range(2f,5f);
            EnemyStateMachine.Animator.CrossFadeInFixedTime(_locomotionHash,AnimatorFadeTime);
            EnemyStateMachine.StatusBar.SetActive(true);
            if (!EnemyStateMachine.isInCombat)
            {
                EnemyStateMachine.isInCombat = true;
                GameManager.Instance.AddAggro();
            }
        }

        public override void Tick(float deltaTime)
        {
            
            EnemyStateMachine.soundTimer -= deltaTime;
            if (EnemyStateMachine.soundTimer <= 0)
            {
                EnemyStateMachine.EnemyAudioController.PlayChaseSound();
                EnemyStateMachine.soundTimer = Random.Range(2f,5f);
            }
            
            if (!IsInChaseRange())
            {
                EnemyStateMachine.SwitchState(new EnemyIdleState(EnemyStateMachine));
                return;
            }
            
            if (IsInAttackRange())
            {
                EnemyStateMachine.SwitchState(new EnemyAttackState(EnemyStateMachine));
                return;
            }
        
            MoveToPlayer(deltaTime);
            FacePlayer();
            EnemyStateMachine.Animator.SetFloat(_speedHash,1,AnimatorDampTime,deltaTime);
        }

        private void MoveToPlayer(float deltaTime)
        {
            if (EnemyStateMachine.Agent.isOnNavMesh)
            {
                EnemyStateMachine.Agent.destination = EnemyStateMachine.Player.transform.position;
                Move(EnemyStateMachine.Agent.desiredVelocity.normalized * EnemyStateMachine.MovementSpeed, deltaTime);
            }
            EnemyStateMachine.Agent.velocity = EnemyStateMachine.Controller.velocity;
        }

        public override void Exit()
        {
            EnemyStateMachine.Agent.ResetPath();
            EnemyStateMachine.Agent.velocity = Vector3.zero;
        }
        private bool IsInAttackRange()
        {
            if (EnemyStateMachine.PlayerHealth.IsDead)
            {
                return false;
            }
            float playerDistanceSqr = (EnemyStateMachine.Player.transform.position - EnemyStateMachine.transform.position).sqrMagnitude;
            return playerDistanceSqr <= EnemyStateMachine.AttackRange * EnemyStateMachine.AttackRange;
        }
    }
}
