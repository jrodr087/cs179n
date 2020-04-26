using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using UnityEngine;

[System.Serializable]
public class Game
{
    public static Game current;
    public Character player;

    public Game() 
    {
        player = new Character();
    }
}
