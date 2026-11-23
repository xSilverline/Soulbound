using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SliderHandler : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI text;
        
        public void UpdateText()
        {
            text.text = Mathf.RoundToInt(slider.value*100) + "%";
        }
    }
}
