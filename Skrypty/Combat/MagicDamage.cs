using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class MagicDamage : MonoBehaviour
    {
        [SerializeField] private Collider areaCollider;
        private List<Collider> _usedCollider = new List<Collider>();
        private int _damage;
        private float _knockbackForce;
        private void OnEnable()
        {
            _usedCollider.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) {return;}
            if (_usedCollider.Contains(other)) {return;}
            _usedCollider.Add(other);
            if (other.TryGetComponent<Health>(out Health health))
            {
                health.DealMagicDamage(_damage);
            }
            if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.position - areaCollider.transform.position).normalized;
                forceReceiver.AddForce(direction * _knockbackForce);
            }
        }

        public void SetAttack(int damage, float knockbackForce)
        {
            _damage = damage;
            _knockbackForce = knockbackForce;
        }
    }
}
