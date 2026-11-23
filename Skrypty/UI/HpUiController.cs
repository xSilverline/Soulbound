using Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HpUiController : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI HpPercentage;

        public void UpdateSlider()
        {
            float currentHealth = health.GetCurrentHealth();
            float maxHealth = health.GetMaxHealth();
            var percentage = currentHealth/maxHealth * 100;
            slider.value = (maxHealth - currentHealth)/maxHealth;
            HpPercentage.text = Mathf.RoundToInt(percentage) + "%";
        }
    }
}
