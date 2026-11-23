using System;
using Combat;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class BossAreaEntrance : MonoBehaviour
    {
        [SerializeField] private GameObject boss;
        [SerializeField] private GameObject bossEntrance;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private GameObject bossUIHp;
        
        private bool _areaEntered = false;
        private void BossSpawn()
        {
            audioManager.SetPlaylist(audioManager.bossPlaylist);
            bossUIHp.SetActive(true);
            bossEntrance.SetActive(true);
            this.gameObject.SetActive(false);
        }
        private void Awake()
        {
            _areaEntered = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (_areaEntered) {return;}
            if (!other.CompareTag("Player"))
                return;
            _areaEntered = true;
            BossSpawn();
            // boss.SetActive(true);
        }
        public bool CheckAggro()
        {
            return _areaEntered;
        }


    }
}
