using System;
using System.Collections;
using UnityEngine;

namespace Combat
{
    public class FireAttack : MonoBehaviour
    {
    
        [SerializeField] private GameObject flamethrower;
        [SerializeField] private GameObject damageArea;
        [SerializeField] private ParticleSystem particleAnimation;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject groundMark;
        [SerializeField] private float timeToSpawn = 3f;
        [SerializeField] private int damage = 50;
        [SerializeField] private float knockbackForce = 20f;
        [SerializeField] private MagicDamage magicDamage;

        // public event Action OnFlamethrowerFinished;
        public void ActivateFlame()
        {
            flamethrower.SetActive(true);
            magicDamage.SetAttack(damage,knockbackForce);
            StartCoroutine(SpawnGroundMark());
        }

        private IEnumerator SpawnGroundMark()
        {
            groundMark.SetActive(true);
            yield return new WaitForSeconds(timeToSpawn);
            groundMark.SetActive(false);
            particleAnimation.Play();
            damageArea.SetActive(true);
            yield return new WaitForSeconds(1.4f);
            damageArea.SetActive(false);
            particleAnimation.Stop();
            flamethrower.SetActive(false);
            
            // OnFlamethrowerFinished?.Invoke();  
        }

    }
}
