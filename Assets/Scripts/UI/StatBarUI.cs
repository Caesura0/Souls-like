using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JS
{
    public class StatBarUI : MonoBehaviour
    {
        Slider slider;


        protected virtual void Awake()
        {
            if(slider == null)
            {
                slider = GetComponent<Slider>();
            }
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            //anytime you increase max stamina we want to refill it
            slider.value = maxValue;
        }

    }
}
