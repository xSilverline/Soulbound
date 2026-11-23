using UnityEngine;

namespace StateMachines.Boss
{
    public class BossWaitingState : BossBaseState
    {
        private readonly int _locomotionHash = Animator.StringToHash("Locomotion");
        private readonly int _roarHash = Animator.StringToHash("Roar");
        private const string RoarTag = "Roaring";
        private readonly int _speedHash = Animator.StringToHash("Speed");
        private const float AnimatorDampTime = 0.1f;
        private const float AnimatorFadeTime = 0.1f;
        private bool _isRoaring = false;

        public BossWaitingState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }


        public override void Enter()
        {
            BossStateMachine.Animator.CrossFadeInFixedTime(_locomotionHash,AnimatorFadeTime);
        }

        public override void Tick(float deltaTime)
        {
            if (BossStateMachine.BossAreaEntrance.CheckAggro())
            {
                if(!_isRoaring)
                {
                    BossStateMachine.Animator.CrossFadeInFixedTime(_roarHash, AnimatorFadeTime);
                    _isRoaring = true;
                }
                if(GetNormalizedTime(BossStateMachine.Animator,RoarTag) >= 1f)
                {
                    BossStateMachine.SwitchState(new BossIdleState(BossStateMachine));
                    return;
                }
            }
            BossStateMachine.Animator.SetFloat(_speedHash,0,AnimatorDampTime,deltaTime);
        }

        public override void Exit()
        {
        
        }
    }
}
