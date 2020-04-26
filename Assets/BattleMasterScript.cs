using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Battler
{
    public int hp, en, maxhp, maxen, att, def, spd, lvl;
    public GameObject sprite;
    public bool isPlayerControlled;
    public Battler()
    {
        hp = en = maxhp = maxen = att = def = spd = lvl = 0;
    }
}
public class BattleMasterScript : MonoBehaviour
{

    private enum states {battleintro,playerturn,playerattack,enemyturn,enemyattack,dead};
    private states currstate;
    public BattleTopBoardScript topboard;
    public Battler player = new Battler();
    public Battler enemy = new Battler();

    // Start is called before the first frame update
    void Start()
    {
        currstate = states.battleintro;
        Debug.Log("Battlemaster initialized");
        topboard.UpdateString("The Spiky Vroomer approached!");
        player.hp = 100;
        player.en = 100;
        player.att = 10;
        enemy.def = 5;
        enemy.hp = 30;
        player.sprite = GameObject.Find("PlayerBattleSprite");
        enemy.sprite = GameObject.Find("EnemyBattleSprite");
        GameObject attack = (GameObject)Instantiate(Resources.Load("Prefabs/DefaultAttackObject"));
        attack.transform.position = enemy.sprite.transform.position - new Vector3(0,.001f,1);
        attack.GetComponent<DefaultAttackScript>().aggressor = player;
        attack.GetComponent<DefaultAttackScript>().target = enemy;
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
    void CreateDamageText(int dmg, GameObject sprite)
    {
        GameObject dmgText = (GameObject)Instantiate(Resources.Load("Prefabs/DamageText"));
        DamageText txt = dmgText.GetComponent<DamageText>();
        txt.dmg = dmg;
        Vector2 pos = sprite.transform.position;
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos) - new Vector3(320,180,0);
        dmgText.GetComponent<RectTransform>().anchorMin = viewportPoint;
        dmgText.GetComponent<RectTransform>().anchorMax = viewportPoint;
        dmgText.transform.SetParent(GameObject.Find("Canvas").transform,false);
    }
    public void DamageBattler(int dmg, Battler btl)
    {
        btl.hp -= dmg;
        CreateDamageText(dmg, btl.sprite);
    }

}
