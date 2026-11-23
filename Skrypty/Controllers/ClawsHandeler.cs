using UnityEngine;

namespace Controllers
{
    public class ClawsHandeler : MonoBehaviour
    {

        [SerializeField] private GameObject clawsLogicLeft;
        [SerializeField] private GameObject clawsLogicRight;

        public void EnableWeaponLeft()
        {
            clawsLogicLeft.SetActive(true);
        }

        public void DisableWeaponLeft()
        {
            clawsLogicLeft.SetActive(false);
        }
    
        public void EnableWeaponRight()
        {
            clawsLogicRight.SetActive(true);
        }

        public void DisableWeaponRight()
        {
            clawsLogicRight.SetActive(false);
        }
    
    }
}
