using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class WeaponDamage : MonoBehaviour
    {
        [SerializeField] private Collider playerCollider;
        private List<Collider> _usedCollider = new List<Collider>();
        private int _damage;
        private float _knockbackForce;
        private void OnEnable()
        {
            _usedCollider.Clear();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other == playerCollider) {return;}
            if (_usedCollider.Contains(other)) {return;}
            _usedCollider.Add(other);
            if (other.TryGetComponent<Health>(out Health health))
            {   
                health.DealDamage(_damage);
            }
            if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.position - playerCollider.transform.position).normalized;
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
