using Controllers;

namespace StateMachines.Player
{
    public class PlayerDeadState : PlayerBaseState
    {
        public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.Ragdoll.ToggleRagdoll(true);
            StateMachine.WeaponDamage.gameObject.SetActive(false);
            AudioManager.Current?.Death();
        }

        public override void Tick(float deltaTime)
        {
        
        }

        public override void Exit()
        {
        
        }
    }
}
