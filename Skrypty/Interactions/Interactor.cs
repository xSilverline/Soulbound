using System;
using StateMachines;
using StateMachines.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactions
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] public PlayerStateMachine stateMachine;
        [SerializeField] public float interactionDistance = 3f;
        [SerializeField] public LayerMask interactionMask;
        private IInteractable _current;
        private Collider[] _results = new Collider[20];

        private void OnEnable()
        {
            stateMachine.InputReader.InteractEvent += OnInteract;
        }

        private void Update()
        {
            _current = GetInteractable();
        }

        private IInteractable GetInteractable()
        {
            
           int count = Physics.OverlapSphereNonAlloc(transform.position, interactionDistance, _results, interactionMask, QueryTriggerInteraction.Collide);
           Interactable closest = null;
           float bestDistance = float.MaxValue;
           
           for( int i = 0; i < count; i++ )
           {
               var hit = _results[i];
               if( !hit ) continue;
               IInteractable interactable = hit.GetComponentInParent<IInteractable>();
               if(interactable == null) continue;
               if(!interactable.CanInteract()) continue;
               var distance = (hit.transform.position - transform.position).sqrMagnitude;
               
               if( distance < bestDistance )
               {
                   bestDistance = distance;
                   closest = hit.GetComponentInParent<Interactable>();
               }
           }
           
           return closest;
        }

        private void OnInteract()
        {
            if (_current != null && _current.CanInteract())
            {
                _current.Interact();
            }
        }
        
        private void OnDisable()
        {
            stateMachine.InputReader.InteractEvent -= OnInteract;
        }
        
        private void OnDestroy()
        {
            OnDisable();
        }
        
    }
}
