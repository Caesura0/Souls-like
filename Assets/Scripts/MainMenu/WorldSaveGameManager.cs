using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JS
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance;

        public PlayerManager player;


        //SaveLoad
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [SerializeField] int worldSceneIndex = 1;

        //CurrentCharacterData
        public CharacterSlots currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        string saveFileName;

        //SaveDataWriter

        SaveFileDataWriter saveFileDataWriter;




        //CharaterSlots

        public CharacterSaveData characterSlot1;
        public CharacterSaveData characterSlot2;
        public CharacterSaveData characterSlot3;
        public CharacterSaveData characterSlot4;
        public CharacterSaveData characterSlot5;
        public CharacterSaveData characterSlot6;
        public CharacterSaveData characterSlot7;
        public CharacterSaveData characterSlot8;
        public CharacterSaveData characterSlot9;


        public void Awake()
        {
            //There should only be on instance of this. If another is accidently created, we destroy the new one
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            LoadAllCharacterProfiles();
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }
            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public void TryCreateNewGame()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUses(CharacterSlots.characterSlot1);


            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlots.characterSlot1;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUses(CharacterSlots.characterSlot2);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlots.characterSlot2;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUses(CharacterSlots.characterSlot3);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlots.characterSlot3;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;
            }

            TitleScreenManager.Instance.DisplayNoFreeCharacterSlotsPopUp();
        }

        public void LoadGame()
        {
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUses(currentCharacterSlotBeingUsed);
            saveFileDataWriter = new SaveFileDataWriter();
            //Generally works on multiple machine types
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUses(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
            
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        void LoadAllCharacterProfiles()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUses(CharacterSlots.characterSlot1);
            characterSlot1 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUses(CharacterSlots.characterSlot2);
            characterSlot2 = saveFileDataWriter.LoadSaveFile();

            //do this for all save slots
        }

        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUses(CharacterSlots characterSlot )
        {
            string fileName = "";
            switch(characterSlot)
            {
                case CharacterSlots.characterSlot1:
                    fileName = "CharacterSlot1";

                    break;

                //add others
                default:
                    break;

            }
            return fileName;
        }


        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperator = SceneManager.LoadSceneAsync(worldSceneIndex);
            player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);
            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }



    }

}
