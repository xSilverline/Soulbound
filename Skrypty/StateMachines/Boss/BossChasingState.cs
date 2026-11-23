using UnityEngine;

namespace StateMachines.Boss
{
    public class BossChasingState : BossBaseState
    {

        private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
        private readonly int _speedHash = Animator.StringToHash("Speed");
        private const float AnimatorDampTime = 0.1f;
        private const float AnimatorFadeTime = 0.1f;
        private float _chaseTimer;

        public BossChasingState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        
        public override void Enter()
        {
            BossStateMachine.Animator.CrossFadeInFixedTime(_locomotionHash,AnimatorFadeTime);
            _chaseTimer = 10f;
        }

        public override void Tick(float deltaTime)
        {
            _chaseTimer -= deltaTime;
            if (_chaseTimer <= 0)
            {
                var giveUpChase = Random.Range(0,2);
                if (giveUpChase == 0)
                {
                    BossStateMachine.SwitchState(new BossFireAttackState(BossStateMachine));
                }
                else
                {
                    BossStateMachine.SwitchState(new BossCrystalAttackState(BossStateMachine));
                }

                return;
            }

            if (IsInAttackRange())
            {
                if(BossStateMachine.nextAttack == 0)
                    BossStateMachine.SwitchState(new BossLeftAttackState(BossStateMachine));
                else
                {
                    BossStateMachine.SwitchState(new BossRightAttackState(BossStateMachine));
                }
                return;
            }
            
            MoveToPlayer(deltaTime);
            FacePlayer();
            BossStateMachine.Animator.SetFloat(_speedHash,1,AnimatorDampTime,deltaTime);
        }

        public override void Exit()
        {
            BossStateMachine.Agent.ResetPath();
            BossStateMachine.Agent.velocity = Vector3.zero;
            
        }
        private void MoveToPlayer(float deltaTime)
        {
            if (BossStateMachine.Agent.isOnNavMesh)
            {
                BossStateMachine.Agent.destination = BossStateMachine.Player.transform.position;
                Move(BossStateMachine.Agent.desiredVelocity.normalized * BossStateMachine.MovementSpeed, deltaTime);
            }
            BossStateMachine.Agent.velocity = BossStateMachine.Controller.velocity;
        }

        private bool IsInAttackRange()
        {
            if(BossStateMachine.PlayerHealth.IsDead)
                return false;

            float playerDistanceSqr = (BossStateMachine.Player.transform.position - BossStateMachine.transform.position)
                .sqrMagnitude;
            return playerDistanceSqr <= BossStateMachine.HandAttackRange * BossStateMachine.HandAttackRange;
        }
    }
}
