using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class TitleScreenLoadMenuInputManager : MonoBehaviour
    {
        PlayerControls playerControls;

        [SerializeField] bool deleteCharactorSlot = false;


        private void Update()
        {
            if(deleteCharactorSlot)
            {
                deleteCharactorSlot = false;
                TitleScreenManager.Instance.AttemptToDeleteCharacterSlot();
            }
        }

        private void OnEnable()
        {
            if(playerControls != null)
            {
                playerControls = new PlayerControls();
                playerControls.UI.x.performed += i => deleteCharactorSlot = true;
            }
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}
