using UnityEngine;

namespace StateMachines.Player
{
    public abstract class PlayerBaseState : State
    {  
        protected PlayerStateMachine StateMachine;

        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this.StateMachine = stateMachine;
        }
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            StateMachine.CharacterController.Move((motion + StateMachine.ForceReceiver.Movement) * deltaTime);
        }

        protected void FaceTarget()
        {
            if(!StateMachine.Targeter.CurrentTarget) {return;}
            
            Vector3 lookPosition = StateMachine.Targeter.CurrentTarget.transform.position - StateMachine.transform.position;
            lookPosition.y = 0f;
            StateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
        }
        
        protected void ReturnToLocomotion()
        {
            if (StateMachine.Targeter.CurrentTarget)
            {
                StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
            }
            else
            {
                StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
            }
        }
    }
}
