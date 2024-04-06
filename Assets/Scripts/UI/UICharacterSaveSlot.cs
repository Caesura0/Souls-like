using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace JS
{
    public class UICharacterSaveSlot : MonoBehaviour
    {
        SaveFileDataWriter saveFileDataWriter;

        //game slot
        public CharacterSlots characterSlot;

        //charactor info
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timePlayed;

        private void OnEnable()
        {
            LoadSavedSlot();
        }



        private void LoadSavedSlot()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            switch(characterSlot)
            {
                case CharacterSlots.characterSlot1:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUses(characterSlot);
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot1.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;

                case CharacterSlots.characterSlot2:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUses(characterSlot);
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot2.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlots.characterSlot3: 
                    break;
                case CharacterSlots.characterSlot4:
                    break;
                case CharacterSlots.characterSlot5: 
                    break;
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.Instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.Instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreenManager.Instance.SelectCharactorSlot(characterSlot);
        }
    }
}
