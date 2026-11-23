using UnityEngine;

namespace StateMachines.Boss
{
    public abstract class BossBaseState : State
    {
        protected BossStateMachine BossStateMachine;

        public BossBaseState(BossStateMachine stateMachine)
        {
            BossStateMachine = stateMachine;
        }

        protected bool IsInChaseRange()
        {
            if (BossStateMachine.PlayerHealth.IsDead)
            {
                return false;           
            }
            float playerDistanceSqr = (BossStateMachine.Player.transform.position - BossStateMachine.transform.position).sqrMagnitude;
            
            return playerDistanceSqr <= BossStateMachine.PlayerChaseRange * BossStateMachine.PlayerChaseRange;
        }
        
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            BossStateMachine.Controller.Move((motion + BossStateMachine.ForceReceiver.Movement) * deltaTime);
        }

        protected void FacePlayer()
        {
            Vector3 lookPosition = BossStateMachine.Player.transform.position - BossStateMachine.transform.position;
            lookPosition.y = 0f;
            BossStateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
        }
        
    }
}
