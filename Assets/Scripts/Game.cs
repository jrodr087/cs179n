using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using UnityEngine;

[System.Serializable]
public class Game
{
    public static Game current;
    public float positionX;
    public float positionY;
    public Game(PlayerData player) 
    {
        GameObject temp = GameObject.Find("Player");
        positionX = temp.transform.position.x;
        positionY = temp.transform.position.y;

        
    }
}
