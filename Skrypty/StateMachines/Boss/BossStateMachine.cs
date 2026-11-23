using AI;
using Combat;
using Combat.Targeting;
using Controllers;
using StateMachines.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace StateMachines.Boss
{
    public class BossStateMachine : StateMachine
    {

         [field: SerializeField] public Animator Animator { get; private set; }
        
        public GameObject Player { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        [field: SerializeField] public WeaponDamage WeaponDamageLeft {get; private set;}
        [field: SerializeField] public WeaponDamage WeaponDamageRight {get; private set;}
        [field: SerializeField] public float PlayerChaseRange { get; private set; }
        [field: SerializeField] public float MagicAttackRange { get; private set; }
        [field: SerializeField] public float HandAttackRange { get; private set; }
        [field: SerializeField] public int AttackDamage { get; private set; }
        [field: SerializeField] public float AttackKnockback { get; private set; }
        [field: SerializeField] public FireAttack FireAttack { get; private set; }
        [field: SerializeField] public CrystalAttack CrystalAttack { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public Target Target { get; private set;}

        [field:SerializeField] public GameObject StatusBar {get; private set;}
        [field:SerializeField] public int HealingValue {get; private set;}
        [field: SerializeField] public AudioSource AudioSource { get; private set; }
        [field: SerializeField] public AudioClip RoarClip {get; private set;}
        [field: SerializeField] public AudioClip AttackClip {get; private set;}
        [field: SerializeField] public BossAreaEntrance BossAreaEntrance {get; private set;}
        [field: SerializeField] public APIBossController APIBossController {get; private set;}
        [field: SerializeField] public TextMeshProUGUI SkillText {get; private set;}
        [field: SerializeField] public ParticleSystem HitLeftParticles {get; private set;}
        [field: SerializeField] public ParticleSystem HitRightParticles {get; private set;}
        public bool isInCombat = false;
        public float soundTimer;
        public Health PlayerHealth { get; private set; }
        public string[] attackList;
        
        public bool isEnraged = false;
        public int nextAttack;
        
        
        private void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            PlayerHealth = Player.GetComponent<Health>();
            Agent.updatePosition = false;
            Agent.updateRotation = false;
            
            SwitchState(new BossWaitingState(this));
        }

        public void HoldAnimation()
        {
            Animator.speed = 0;
        }

        private void OnEnable()
        {
            APIBossController.WordsFinishedEvent += GetAttackList;
            Health.OnTakeDamage += HandleTakeDamage;
            Health.OnDie += HandleDeath;
        }

        private void OnDisable()
        {
            APIBossController.WordsFinishedEvent -= GetAttackList;
            Health.OnTakeDamage -= HandleTakeDamage;
            Health.OnDie -= HandleDeath;
        }
        
        public void PlayRoar()
        {
            AudioSource.PlayOneShot(RoarClip);
        }

        public void StopSound()
        {
            AudioSource.Stop();
        }

        private void GetAttackList()
        {
            attackList = APIBossController.GetWords();
        }

        private void HandleTakeDamage()
        {
            HitLeftParticles.Play();
            HitRightParticles.Play();
            // SwitchState(new EnemyImpactState(this));
        }
        

        private void HandleDeath()
        {
            SwitchState(new BossDeadState(this));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, PlayerChaseRange);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, MagicAttackRange);
        }
    }
}
