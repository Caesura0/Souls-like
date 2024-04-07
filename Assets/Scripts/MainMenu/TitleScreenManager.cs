using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;


namespace JS
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance;

        
        [SerializeField]  GameObject titleScreenMainMenu;
        [SerializeField]  GameObject titleScreenLoadMenu;


        //buttons
        [SerializeField] Button mainMenuiNewGameButton;
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button noCharactorSlotsOkayButton;
        [SerializeField] Button deleteCharacterPopupConfirmButton;

        //pop ups
        [SerializeField] GameObject noCharactorSlotsPopUp;
        [SerializeField] GameObject deleteCharactorSlotsPopUp;

        //save slots


        public CharacterSlots currentlySelectedSlot = CharacterSlots.NO_SLOT;




        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }
        //This function is called in a UI button, StartNewGameButton
        public void StartNewGame()
        {
            WorldSaveGameManager.Instance.TryCreateNewGame();

        }

        public void OpenLoadGameMenu()
        {
            titleScreenMainMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);
            loadMenuReturnButton.Select();
        }



        public void CloseLoadGameMenu()
        {
            titleScreenLoadMenu.SetActive(false);
            titleScreenMainMenu.SetActive(true);
            mainMenuLoadGameButton.Select();

        }

        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            noCharactorSlotsPopUp.SetActive(true);
            noCharactorSlotsOkayButton.Select();
        }

        public void CloseNoFreeCharacterSlotsPopUp()
        {
            noCharactorSlotsPopUp.SetActive(false);
            mainMenuiNewGameButton.Select();
        }

        public void SelectCharactorSlot(CharacterSlots slot)
        {
            currentlySelectedSlot = slot;
        }


        public void SelectNoSlot()
        {
            currentlySelectedSlot = CharacterSlots.NO_SLOT;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if(currentlySelectedSlot != CharacterSlots.NO_SLOT)
            {
                deleteCharactorSlotsPopUp.SetActive(true);
                deleteCharacterPopupConfirmButton.Select();
            }

        }

        //hooks to UI button
        public void DeleteCharactorSlot()
        {
            deleteCharactorSlotsPopUp.SetActive(false);
            WorldSaveGameManager.Instance.DeleteGame(currentlySelectedSlot);
            //cheater way to refresh the saves
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);


            loadMenuReturnButton.Select();
        }

        public void CloseDeleteCharacterPopup()
        {
            deleteCharactorSlotsPopUp.SetActive(false);
            loadMenuReturnButton.Select();
        }


    }


}
