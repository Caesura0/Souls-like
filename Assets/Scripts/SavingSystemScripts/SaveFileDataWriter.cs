using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace JS
{
    public class SaveFileDataWriter 
    {
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";

        public bool CheckToSeeIfFileExists()
        {
            return File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)) ;

        }

        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        public void CreateNewCharacterSaveFile(CharacterSaveData characterSaveData)
        {
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("creating save file at path: " + savePath);
                string dataToStore = JsonUtility.ToJson(characterSaveData, true);

                using(FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using(StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch(Exception e)
            {
                Debug.LogError("Saving error in saveFileDataWriter");
            }
        }

        public CharacterSaveData LoadSaveFile()

        {
            CharacterSaveData characterSaveData = null;
            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    characterSaveData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);

                }




                catch (Exception e)
                {
                    Debug.Log("loading save data error");
                    
                }
                
            }
            return characterSaveData;
        }


    }
}
