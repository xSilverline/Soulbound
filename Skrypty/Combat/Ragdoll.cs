using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Combat
{
    public class Ragdoll : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public CharacterController Controller { get; private set; }
        private Collider[] _allColliders;
        private Rigidbody[] _allRigidbodies;
        void Start()
        {
            _allColliders = GetComponentsInChildren<Collider>(true);
            _allRigidbodies = GetComponentsInChildren<Rigidbody>(true);
            
            ToggleRagdoll(false);
        
        }

        public void ToggleRagdoll(bool isRagdoll)
        {
            foreach (var collider in _allColliders)
            {
                if (collider.gameObject.CompareTag("Ragdoll"))
                {
                    collider.enabled = isRagdoll;
                }
            }
            foreach (var rigidbody in _allRigidbodies)
            {
                if (rigidbody.gameObject.CompareTag("Ragdoll"))
                {
                    rigidbody.isKinematic = !isRagdoll;
                    rigidbody.useGravity = isRagdoll;
                }
            }
            Controller.enabled = !isRagdoll;
            Animator.enabled = !isRagdoll;
        }

    }
}
