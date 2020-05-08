using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attacks
{
    public int[] weights =
    {

    };
    public string[] attackObjs =
    {

    };
};

public class Enemy
{
    public int hp, en, maxhp, maxen, att, def, spd, lvl;
    public string name;
    public string spritePath;
    public Attacks atks;
};
public class EnemyDirectory
{
    public enum EnemyIndex { };
    public Enemy[] dir =
    {

    };
}


public class Battler
{
    public int hp, en, maxhp, maxen, att, def, spd, lvl;
    public string name;
    private float radius = 0.0f;
    private bool shake = false;
    private bool flashing = false;
    private float flashtimer = 0.0f;
    private bool launched = false;
    private float shakeT = 0.0f;
    public GameObject sprite;
    private Vector3 pos;
    public bool isPlayerControlled;
    Renderer r;
    public Battler()
    {
        hp = en = maxhp = maxen = att = def = spd = lvl = 0;
    }
    public Battler(GameObject sp, string name)
    {
        hp = en = maxhp = maxen = att = def = spd = lvl = 0;
        this.name = name;
        sprite = sp;
        pos = sprite.transform.position;
    }
    public Battler(string name, int maxhp, int maxen, int att, int def, int spd, int lvl)
    {
        this.name = name;
        this.maxhp = hp = maxhp;
        this.maxen = en = maxen;
        this.att = att;
        this.def = def;
        this.spd = spd;
        this.lvl = lvl;
    }
    public void UpdateBattler()
    {
        r = sprite.GetComponent<Renderer>();
        if (shake)
        {
            radius -= 0.0035f;
            shakeT += Time.deltaTime;
            float currradius = radius * Mathf.Sin(shakeT * 12);
            if (radius <= 0.0f)
            {
                shake = false;
                sprite.transform.position = pos;
                shakeT = 0.0f;
            }
            else
            {
                sprite.transform.position = pos + new Vector3(currradius, 0, 0);
            }
        }
        if (flashing)
        {
            r.enabled = !r.enabled;
            flashtimer -= Time.deltaTime;
            if (flashtimer <= 0.0f)
            {
                r.enabled = true;
                flashing = false;
            }
        }
        if (launched)
        {
            if (!isPlayerControlled)
            {
                sprite.transform.Rotate(0, 0, 15);
                sprite.transform.position += new Vector3(1, 1.5f, 0) * Time.deltaTime;
                sprite.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;
                if (sprite.transform.localScale.x <= 0.0f)
                {
                    r.enabled = false;
                }
            }
            else
            {
                r.enabled = false;
            }
        }
    }
    public void StartShake()
    {
        shake = true;
        shakeT = 0.0f;
        radius = 1.0f;
    }
    public void StartFlashing(float flashtime)
    {
        flashtimer = flashtime;
        flashing = true;
    }
    public void Launch()
    {
        launched = true;
    }
}



public class BattleMasterScript : MonoBehaviour
{
    private enum states {battleintro,playerturn,playerattack,playerturnend,enemyturn,enemyattack,enemyturnend,dead,battlewon};
    private states currstate;
    public BattleTopBoardScript topboard;
    public Battler player;
    public Battler enemy;
    public List<Battler> playerControlledBattlers;
    public List<Battler> enemies;
    public AudioSource audio;
    public GameObject choiceRadial;
    public Text playerhp;
    public Text playeren;
    public Text playermaxhp;
    public Text playermaxen;
    private float generalTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = new Battler(GameObject.Find("PlayerBattleSprite"), "Player");
        enemy = new Battler(GameObject.Find("EnemyBattleSprite"), "Spiky Vroomer");
        audio = gameObject.AddComponent<AudioSource>();
        currstate = states.battleintro;
        Debug.Log("Battlemaster initialized");
        topboard.UpdateString("The Spiky Vroomer approached!");
        PlayerData pd = GameObject.Find("Player").GetComponent<PlayerData>();
        player.hp = pd.stats.hp;
        player.maxhp = pd.stats.maxhp;
        player.en = pd.stats.en;
        player.maxen = pd.stats.maxen;
        player.att = pd.stats.off;
        player.def = pd.stats.def;
        player.spd = pd.stats.spd;
        player.isPlayerControlled = true;
        enemy.def = 5;
        enemy.hp = 30;
        enemy.att = 9;
        enemy.spd = 20;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currstate)
        {
            case states.battleintro:
                generalTimer += Time.deltaTime;
                if (generalTimer >= 1.0f)
                {
                    generalTimer = 0.0f;
                    currstate = states.playerturn;
                    topboard.UpdateString("It's your turn.");
                    player.StartFlashing(1.0f);
                }
                break;
            case states.dead:
                break;
            case states.playerturn:
                choiceRadial.SetActive(true);
                break;
            case states.playerattack:
                break;
            case states.playerturnend:
                generalTimer += Time.deltaTime;
                if (generalTimer >= 1.0f)
                {
                    generalTimer = 0.0f;
                    currstate = states.enemyturn;
                    topboard.UpdateString("The " + enemy.name + " is up next.");
                    enemy.StartFlashing(1.5f);
                }
                break;
            case states.enemyturn:
                generalTimer += Time.deltaTime;
                if (generalTimer >= 2.0f)
                {
                    generalTimer = 0.0f;
                    currstate = states.enemyattack;
                    topboard.UpdateString("The " + enemy.name + " attacked!");
                    EnemyAttack();
                }
                break;
            case states.enemyattack:
                break;
            case states.enemyturnend:
                generalTimer += Time.deltaTime;
                if (generalTimer >= 1.0f)
                {
                    generalTimer = 0.0f;
                    currstate = states.playerturn;
                }
                break;
            case states.battlewon:
                generalTimer += Time.deltaTime;
                if (generalTimer >= 3.0f)
                {
                    generalTimer = 0.0f;
                    currstate = states.dead;
                    EndBattle();
                }
                break;
            default:
                currstate = states.battleintro;
                break;

        }
        player.UpdateBattler();
        enemy.UpdateBattler();
        playerhp.text = player.hp.ToString();
        playermaxhp.text = player.maxhp.ToString();
        playeren.text = player.en.ToString();
        playermaxen.text = player.maxen.ToString();
    }
    void CreateDamageText(int dmg, GameObject sprite,bool critical)
    {
        GameObject dmgText = (GameObject)Instantiate(Resources.Load("Prefabs/DamageText"));
        audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Hit"));
        DamageText txt = dmgText.GetComponent<DamageText>();
        txt.dmg = dmg;
        txt.critical = critical;
        Vector2 pos = sprite.transform.localPosition;
        Vector2 viewportPoint = 32 * pos;
        dmgText.GetComponent<RectTransform>().anchoredPosition = viewportPoint;
        dmgText.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }
    public void DamageBattler(int dmg, Battler btl, bool critical)
    {
        btl.hp -= dmg;
        topboard.UpdateString("The " + btl.name + " took " + dmg.ToString() + " damage!");
        if (btl.hp > 0)
        {
            btl.StartShake();
        }
        else
        {
            btl.Launch();
            if (btl.name == player.name)
            {
                topboard.UpdateString("You lost...");
                currstate = states.dead;
            }
            if (btl.name == enemy.name)
            {
                topboard.UpdateString("You won!");
                currstate = states.battlewon;
            }
        }
        btl.StartFlashing(3.0f);
        btl.sprite.GetComponent<ParticleSystem>().Emit(dmg);
        CreateDamageText(dmg, btl.sprite,critical);
    }
    public void PlayerAttack()
    {
        GameObject attack = (GameObject)Instantiate(Resources.Load("Prefabs/DefaultAttackObject"));
        attack.transform.position = enemy.sprite.transform.position - new Vector3(0, .001f, 1);
        attack.GetComponent<DefaultAttackScript>().aggressor = player;
        attack.GetComponent<DefaultAttackScript>().target = enemy;
        currstate = states.playerattack;
    }
    public void EnemyAttack()
    {
        GameObject enemyAttack = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyAttack"));
        enemyAttack.GetComponent<AttackAnim>().aggressor = enemy;
        enemyAttack.GetComponent<AttackAnim>().target = player;
        enemyAttack.transform.position = player.sprite.transform.position;
        currstate = states.enemyattack;
    }
    public void EndPlayerAttack()
    {
        if (currstate != states.dead && currstate != states.battlewon)
        {
            currstate = states.playerturnend;
        }
    }
    public void EndEnemyAttack()
    {
        if (currstate != states.dead && currstate != states.battlewon)
        {
            currstate = states.enemyturnend;
        }
    }
    public void EndBattle()
    {
        PlayerMovement ps = GameObject.Find("Player").GetComponent<PlayerMovement>();
        PlayerData pd = GameObject.Find("Player").GetComponent<PlayerData>();
        pd.stats.hp = player.hp;
        pd.stats.en = player.en;
        pd.GiveExp(10);
        ps.LeaveBattle();
    }
}
