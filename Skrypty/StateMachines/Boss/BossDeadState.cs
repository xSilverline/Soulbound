using Controllers;
using UnityEngine;

namespace StateMachines.Boss
{
    public class BossDeadState : BossBaseState
    {
        private readonly int _deadHash = Animator.StringToHash("Death");
        private const float AnimatorFadeTime = 0.1f;
        public BossDeadState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            BossStateMachine.StatusBar.SetActive(false);
            Object.Destroy(BossStateMachine.Target);
            BossStateMachine.WeaponDamageLeft.gameObject.SetActive(false);
            BossStateMachine.WeaponDamageRight.gameObject.SetActive(false);
            BossStateMachine.Animator.CrossFadeInFixedTime(_deadHash,AnimatorFadeTime);
            AudioManager.Current?.BossDeath();
        }

        public override void Tick(float deltaTime)
        {
        
        }

        public override void Exit()
        {
       
        }
    }
}
