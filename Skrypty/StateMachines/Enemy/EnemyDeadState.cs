using Controllers;
using UnityEngine;

namespace StateMachines.Enemy
{
    public class EnemyDeadState : EnemyBaseState
    {
        public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            EnemyStateMachine.StatusBar.SetActive(false);
            EnemyStateMachine.EnemyAudioController.PlayDeathSound();
            EnemyStateMachine.Ragdoll.ToggleRagdoll(true);
            EnemyStateMachine.WeaponDamageLeft.gameObject.SetActive(false);
            EnemyStateMachine.WeaponDamageRight.gameObject.SetActive(false);
            Object.Destroy(EnemyStateMachine.Target);
            if (EnemyStateMachine.isInCombat)
            {
                EnemyStateMachine.isInCombat = false;
                GameManager.Instance.RemoveAggro();
            }
        }

        public override void Tick(float deltaTime)
        {
        
        }

        public override void Exit()
        {
        
        }
    }
}
