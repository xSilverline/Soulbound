using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Combat.Targeting;
using Controllers;
using InputSystem;
using Unity.Cinemachine;


namespace StateMachines.Player

{
    public class PlayerStateMachine : StateMachine
    {
        private static readonly int FreeLookForward = Animator.StringToHash("FreeLookForward");
        private static readonly int FreeLookRight = Animator.StringToHash("FreeLookRight");
        private static readonly int TargetingForward = Animator.StringToHash("TargetingForward");
        private static readonly int TargetingRight = Animator.StringToHash("TargetingRight");
        [field: SerializeField] public InputReader InputReader { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
        [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public float RotationDamp { get; private set; }
        public Transform MainCameraTransform { get; private set; }
        [field: SerializeField] public Targeter Targeter { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public Attack[] Attacks { get; private set; }
        [field: SerializeField] public WeaponDamage WeaponDamage {get; private set;}
        [field: SerializeField] public Health Health { get; private set; }
 
        [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
        [field: SerializeField] public float DodgeDuration { get; private set; }
        [field: SerializeField] public float DodgeDistance { get; private set; }
        [field: SerializeField] public float DodgeCooldown { get; private set; }
        [field: SerializeField] public CinemachineInputAxisController CinemachineInputAxisController { get; private set; }
        [field: SerializeField] public AudioSource AudioSource { get; private set; }
        [field: SerializeField] public AudioClip FootClip {get; private set;}
        [field: SerializeField] public AudioClip BlockClip {get; private set;}
        [field: SerializeField] public AudioClip AttackClip {get; private set;}
        public float PreviousDodgeTime { get; private set; } = -Mathf.Infinity;
        
        private void Start()
        {
            if (Camera.main != null) MainCameraTransform = Camera.main.transform;
            SwitchState(new PlayerFreeLookState(this));
        }
        
        private void OnEnable()
        {
            Health.OnTakeDamage += HandleTakeDamage;
            InputReader.CancelEvent += ShowMenu;
            Health.OnDie += HandleDeath;
        }

        public void Footstep()
        {
            
            if (AudioSource.isPlaying)
            {
                AudioSource.Stop();
            }
            if(AudioSource.volume < 1f)
                AudioSource.volume = 1f;
            
            var freeLookForward = Animator.GetFloat(FreeLookForward);
            var freeLookRight = Animator.GetFloat(FreeLookRight);
            var targetingForward = Animator.GetFloat(TargetingForward);
            var tagetingRight = Animator.GetFloat(TargetingRight);
            if (Mathf.Abs(freeLookForward) > 0.1f || Mathf.Abs(targetingForward) > 0.1f || Mathf.Abs(tagetingRight) > 0.1f || Mathf.Abs(freeLookRight) > 0.1f)
            {
                AudioSource.PlayOneShot(FootClip);
            }
            
        }

        public void PlayAttackSound()
        {
            if (AudioSource.isPlaying)
            {
                AudioSource.Stop();
            }
            AudioSource.volume = 0.1f;
            AudioSource.PlayOneShot(AttackClip);
        }

        public void StopFootstep()
        {
            AudioSource.Stop();
        }

        public void PlayBlockSound()
        {
            if (AudioSource.isPlaying)
            {
                AudioSource.Stop();
            }
            AudioSource.PlayOneShot(BlockClip);
        }

        private void OnDisable()
        {
            Health.OnTakeDamage -= HandleTakeDamage;
            Health.OnDie -= HandleDeath;
            InputReader.CancelEvent -= ShowMenu;
        }

        private void ShowMenu()
        {
            GameManager.Instance.ShowMenu();
        }

        private void HandleTakeDamage()
        {
            SwitchState(new PlayerImpactState(this));
        }

        private void HandleDeath()
        {
            SwitchState(new PlayerDeadState(this));
        }
        
        public void SetDodgeTime(float time)
        {
            PreviousDodgeTime = time;
        }
        
    }
}
