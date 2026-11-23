using System;
using UI;
using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private HpUiController hpUiController;
        private int _currentHealth;
        private bool _isInvulnerable;
        
        public event Action OnTakeDamage;
        public event Action OnDie;
        
        public bool IsDead => _currentHealth <= 0;

        private void Start()
        {
            _currentHealth = maxHealth;
        }

        public void SetInvulnerable(bool isInvulnerable)
        {
            _isInvulnerable = isInvulnerable;
        }
        
        public void Heal(int amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, maxHealth);
            hpUiController.UpdateSlider();
        }
        public int GetCurrentHealth() => _currentHealth;
        public int GetMaxHealth() => maxHealth;
    
        public void DealDamage(int damage)
        {
            if (_currentHealth <= 0)
            {
                return;
            }

            if (_isInvulnerable)
            {
                return;
            }
        
            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            hpUiController.UpdateSlider();
            OnTakeDamage?.Invoke();

            if (_currentHealth <= 0)
            {
                OnDie?.Invoke();
            }
            Debug.Log("Damage dealt - HP: " + _currentHealth + " for "+ this.gameObject.name);
        }

        public void DealMagicDamage(int damage)
        {
            if (_currentHealth <= 0)
            {
                return;
            }
            if (_isInvulnerable)
            {
                damage = Mathf.RoundToInt(damage * 0.5f);
            }
            
            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            hpUiController.UpdateSlider();
            OnTakeDamage?.Invoke();

            if (_currentHealth <= 0)
            {
                OnDie?.Invoke();
            }
            
        }
        public bool IsInvulnerable() => _isInvulnerable;


    }
}
