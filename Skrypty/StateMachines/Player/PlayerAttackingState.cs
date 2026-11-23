using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private float _previousFrameTime;
        private string _lightattackTag = "LAttack";
        private Attack _attack;
        private bool _forceApplied = false;
        public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
        {
            _attack = StateMachine.Attacks[attackIndex];
        }

        public override void Enter()
        {
            StateMachine.WeaponDamage.SetAttack(_attack.Damage, _attack.Knockback);
            StateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, _attack.TransitionDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            FaceTarget();
            float normalizedTime = GetNormalizedTime(StateMachine.Animator, _lightattackTag);


            if (normalizedTime >= 0.7f && normalizedTime >= _previousFrameTime && StateMachine.InputReader.IsBlocking)
            {
                StateMachine.SwitchState(new PlayerBlockingState(StateMachine));
            }
            

            if (normalizedTime >= _previousFrameTime && normalizedTime < 1f)
            {
                if (!StateMachine.Targeter.CurrentTarget)
                {
                    Vector3 forward = StateMachine.MainCameraTransform.forward;
                    forward.y = 0f;
                    forward.Normalize();
                    StateMachine.transform.rotation = Quaternion.Lerp(StateMachine.transform.rotation,Quaternion.LookRotation(forward),deltaTime );
                }
                
                if (normalizedTime >= _attack.ForceTime)
                {
                    TryApplyForce();
                }
                
                if (StateMachine.InputReader.IsAttacking)
                {
                    TryComboAttack(normalizedTime);
                }
                
            }
            else
            {
                if (StateMachine.InputReader.IsBlocking)
                {
                    StateMachine.SwitchState(new PlayerBlockingState(StateMachine));
                }
                
                if (StateMachine.Targeter.CurrentTarget)
                {
                    StateMachine.SwitchState(new PlayerTargetingState(StateMachine));
                }
                else
                {
                    StateMachine.SwitchState(new PlayerFreeLookState(StateMachine));
                }
            }
            _previousFrameTime = normalizedTime;
        }


        public override void Exit()
        {
            
        }

        private void TryApplyForce()
        {
            if (_forceApplied) {return;}
            StateMachine.ForceReceiver.AddForce(StateMachine.transform.forward * _attack.Force);
            _forceApplied = true;
        }
        
        private void TryComboAttack(float normalizedTime)
        {
            if (_attack.ComboStateIndex == -1)
            {
                return;
            }
            
            if(normalizedTime < _attack.ComboAttackTime)
            {
                return;
            }
            StateMachine.SwitchState(new PlayerAttackingState(StateMachine,_attack.ComboStateIndex));
                
        }

        
        
    }
}
