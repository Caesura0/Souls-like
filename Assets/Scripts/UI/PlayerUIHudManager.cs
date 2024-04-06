using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] StatBarUI staminaBar;


        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue( int newValue)
        {
            staminaBar.SetMaxStat(newValue);
        }
    }
}
