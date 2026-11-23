using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class ClawDamage : MonoBehaviour
    {
        [SerializeField] private Collider playerCollider;
        private List<Collider> _usedCollider = new List<Collider>();
        private int _damage;
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
        }

        public void SetAttack(int damage)
        {
            this._damage = damage;
        }
    }
}
