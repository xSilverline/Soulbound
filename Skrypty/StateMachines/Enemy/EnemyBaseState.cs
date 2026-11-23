using UnityEngine;

namespace StateMachines.Enemy
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine EnemyStateMachine;

        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
            EnemyStateMachine = stateMachine;
        }

        protected bool IsInChaseRange()
        {
            if (EnemyStateMachine.PlayerHealth.IsDead)
            {
                return false;           
            }
            float playerDistanceSqr = (EnemyStateMachine.Player.transform.position - EnemyStateMachine.transform.position).sqrMagnitude;
            
            return playerDistanceSqr <= EnemyStateMachine.PlayerChaseRange * EnemyStateMachine.PlayerChaseRange;
        }



        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            EnemyStateMachine.Controller.Move((motion + EnemyStateMachine.ForceReceiver.Movement) * deltaTime);
        }

        protected void FacePlayer()
        {
            Vector3 lookPosition = EnemyStateMachine.Player.transform.position - EnemyStateMachine.transform.position;
            lookPosition.y = 0f;
            EnemyStateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
        }
    }
}
