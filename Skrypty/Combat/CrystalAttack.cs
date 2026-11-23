using System;
using System.Collections;
using Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat
{
    public class CrystalAttack : MonoBehaviour
    {
        [SerializeField] private GameObject crystal;
        [SerializeField] private GameObject damageArea;
        [SerializeField] private ParticleSystem particleAnimation;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject groundMark;
        [SerializeField] private float timeToSpawn = 3f;
        [SerializeField] private int damage = 50;
        [SerializeField] private float knockbackForce = 20f;
        [SerializeField] private MagicDamage magicDamage;
        
        public event Action OnCrystalAttackFinished;

        public void SpawnCrystal()
        {
            crystal.transform.position = player.transform.position;
            crystal.SetActive(true);
            magicDamage.SetAttack(damage,knockbackForce);
            StartCoroutine(ShowGroundMark());
        }

        private IEnumerator ShowGroundMark()
        {
            groundMark.SetActive(true);
            yield return new WaitForSeconds(timeToSpawn);
            groundMark.SetActive(false);
            particleAnimation.Play();
            yield return new WaitForSeconds(0.5f);
            damageArea.SetActive(true);
            yield return new WaitForSeconds(0.1f); 
            damageArea.SetActive(false);
            yield return new WaitForSeconds(1.4f);
            crystal.SetActive(false);
            OnCrystalAttackFinished?.Invoke();   
        }

    }
}
