using UnityEngine;
using System;

namespace Combat.Targeting
{
    public class Target : MonoBehaviour
    {
        public event Action<Target> OnDestroyed;
        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }
    }
}
