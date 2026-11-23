using StateMachines.Enemy;
using UnityEngine;

namespace Controllers
{
    public class EnemyAudioController : MonoBehaviour
    {
        [field: SerializeField] public AudioClip[] ImpactClips { get; private set; }
        [field: SerializeField] public AudioSource AudioSource { get; private set; }
        [field: SerializeField] public AudioClip AttackClip {get; private set;}
        [field: SerializeField] public AudioClip[] IdleGrawl {get; private set;}
        [field: SerializeField] public AudioClip DeathClip {get; private set;}
        [field: SerializeField] public AudioClip[] ChaseClip {get; private set;}

        public void PlayImpactSound()
        {
            AudioSource.PlayOneShot(ImpactClips[Random.Range(0, ImpactClips.Length)]);
        }

        // public void PlayAttackSound()
        // {
        //     AudioSource.PlayOneShot(AttackClip);
        // }
        
        public void PlayIdleGrawlSound()
        {
            AudioSource.PlayOneShot(IdleGrawl[Random.Range(0, IdleGrawl.Length)]);
        }

        public void PlayDeathSound()
        {
            AudioSource.PlayOneShot(DeathClip);
        }
        public void PlayChaseSound()
        {
            AudioSource.PlayOneShot(ChaseClip[Random.Range(0, ChaseClip.Length)]);
        }
        

        
        
    }
}
