using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace JS
{
    public class CharacterStatManager : MonoBehaviour
    {
        //Stamina Regen
        CharaterManager charater;
        [SerializeField] float staminaRegenDelay = 1.2f;
        [SerializeField] int staminaRegenAmount = 1;
        float staminaRegenTimer = 0f;
        float staminaTickTimer = 0f;

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {
            charater = GetComponent<CharaterManager>();
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0f;
            stamina = endurance * 10;
            return Mathf.RoundToInt(stamina);
        }

        public virtual void ResetStaminaReganTimer(float previousValue, float currentValue)
        {
            if(currentValue < previousValue)
            {
                staminaRegenTimer = staminaRegenDelay;
            }

        }


        public virtual void RegenStamina()
        {
            if (!charater.IsOwner)
            {
                return;
            }

            if (charater.charactorNetworkManager.isSprinting.Value)
            {
                return;
            }
            if (charater.isPerformingAction)
            {
                return;
            }

            staminaRegenTimer += Time.deltaTime;

            if (staminaRegenTimer >= staminaRegenDelay)
            {
                if (charater.charactorNetworkManager.currentStamina.Value < charater.charactorNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;
                    if (staminaTickTimer >= 0.1f)
                    {
                        staminaTickTimer = 0;
                        charater.charactorNetworkManager.currentStamina.Value += staminaRegenAmount;
                    }
                }
            }

        }
    }
}
