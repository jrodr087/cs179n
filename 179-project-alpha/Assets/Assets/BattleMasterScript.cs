using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMasterScript : MonoBehaviour
{

    private enum states {battleintro,playerturn,playerattack,enemyturn,enemyattack,dead};
    private states currstate;
    public BattleTopBoardScript topboard;

    // Start is called before the first frame update
    void Start()
    {
        currstate = states.battleintro;
        Debug.Log("Battlemaster initialized");
        topboard.UpdateString("The Spiky Vroomer approached!");
    }

    // Update is called once per frame
    void Update()
    {
        switch (currstate)
        {
            case states.battleintro:
                topboard.UpdateString("The Spiky Vroomer approached!");
                currstate = states.dead;
                break;
            case states.dead:
                break;
            default:
                currstate = states.battleintro;
                break;

        }
    }
}
