using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    [System.Serializable ]
    public class CharacterSaveData 
    {
        //Scene Index 
        //Intialized as 1, which is my starting scene, this way when i create a new charactor they go to scene one until i save otherwise
        public int sceneIndex = 1;


        public string characterName = "Character";

        //time played
        public float secondsPlayed;


        public float xPosition;    
        public float yPosition;    
        public float zPosition;    



    }
}
