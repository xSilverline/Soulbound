using System;
using Combat;
using Combat.Targeting;
using Controllers;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace StateMachines.Enemy
{
    public class EnemyStateMachine : StateMachine
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        
        public GameObject Player { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        [field: SerializeField] public WeaponDamage WeaponDamageLeft {get; private set;}
        [field: SerializeField] public WeaponDamage WeaponDamageRight {get; private set;}
        [field: SerializeField] public float PlayerChaseRange { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public int AttackDamage { get; private set; }
        [field: SerializeField] public float AttackKnockback { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public Target Target { get; private set;}
        [field: SerializeField] public Ragdoll Ragdoll { get; private set; }

        [field:SerializeField] public GameObject StatusBar {get; private set;}
        [field:SerializeField] public int HealingValue {get; private set;}
        [field:SerializeField] public EnemyAudioController EnemyAudioController {get; private set;}
        [field:SerializeField] public ParticleSystem HitParticles {get; private set;}
        
        public bool isInCombat = false;
        public float soundTimer;
        public Health PlayerHealth { get; private set; }
        
        private void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            PlayerHealth = Player.GetComponent<Health>();
            Agent.updatePosition = false;
            Agent.updateRotation = false;
            SwitchState(new EnemyIdleState(this));
        }

        private void OnEnable()
        {
            Health.OnTakeDamage += HandleTakeDamage;
            Health.OnDie += HandleDeath;
        }

        private void OnDisable()
        {
            Health.OnTakeDamage -= HandleTakeDamage;
            Health.OnDie -= HandleDeath;
        }

        private void HandleTakeDamage()
        {
            HitParticles.Play();
            SwitchState(new EnemyImpactState(this));
        }

        private void HandleDeath()
        {
            SwitchState(new EnemyDeadState(this));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, PlayerChaseRange);
        }
    }
}
