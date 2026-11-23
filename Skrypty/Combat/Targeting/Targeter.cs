using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Targeting
{
    public class Targeter : MonoBehaviour
    {
        private List<Target> _targets = new List<Target>();
        public Target CurrentTarget { get; private set; }
        [field: SerializeField] private CinemachineTargetGroup CineTargetGroup { get; set; }

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Target target))
            {
                return;
            }
            _targets.Add(target);
            target.OnDestroyed += RemoveTarget;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Target target))
            { return; }
            RemoveTarget(target);
        }

        public bool SelectTarget()
        {
            if(_targets.Count <= 0) {return false;}

            Target closestTarget = null;
            var closestDistance = Mathf.Infinity;
            
            foreach (var target in _targets)
            {
                
                Vector3 viewPosition = _mainCamera.WorldToViewportPoint(target.transform.position);
                if (viewPosition.x < 0 || viewPosition.x > 1 || viewPosition.y < 0 || viewPosition.y > 1 || viewPosition.z < 0)
                {
                    continue;
                }

                Vector2 viewPos = viewPosition;
                var toCenter = viewPos - new Vector2(0.5f,0.5f);
                if (!(toCenter.sqrMagnitude < closestDistance)) continue;
                closestDistance = toCenter.sqrMagnitude;
                closestTarget = target;
            }
            
            if(closestTarget == null) { return false; }
            CurrentTarget = closestTarget;
            CineTargetGroup.AddMember(CurrentTarget.transform,1,2);
            return true;
        }

        public void Cancel()
        {
            if(CurrentTarget == null) { return; }
            CineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }
        
        private void RemoveTarget(Target target)
        {
            if (CurrentTarget == target)
            {
                CineTargetGroup.RemoveMember(CurrentTarget.transform);
                CurrentTarget = null;
            }
            _targets.Remove(target);
            target.OnDestroyed -= RemoveTarget;
        }
        
    }
}
