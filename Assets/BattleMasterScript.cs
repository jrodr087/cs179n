using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;



public class BattleMasterScript : MonoBehaviour
{
    private enum states { battleintro, dead, battlewon, turnstart,turnattack,turnend };
    private states currstate;
    private EnemyFactory ef = new EnemyFactory();
    public BattleTopBoardScript topboard;
    public GameObject playerCharaCenter;
    public GameObject enemyCharaCenter;
    public Battler player;
    public Battler enemy;
    public List<Battler> playerControlledBattlers = new List<Battler>();
    public List<Battler> enemies = new List<Battler>();
    public new AudioSource audio;
    public GameObject choiceRadial;
    public Text playerhp;
    public Text playeren;
    public Text playermaxhp;
    public Text playermaxen;
    private Battler currbattler;
    private List<Battler> battlerQueue = new List<Battler>();
    private float generalTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.AddComponent<AudioSource>();
        Debug.Log("Battlemaster initialized");
    }

    // Update is called once per frame
    void Update()
    {
        switch (currstate)
        {
            case states.dead:
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
            case states.turnstart:
                {
                    currbattler = battlerQueue[0];
                    battlerQueue.RemoveAt(0);
                    if (!currbattler.isPlayerControlled)
                    {
                        //generalTimer += Time.deltaTime;
                        //if (generalTimer >= 2.0f)
                        //{
                        generalTimer = 0.0f;
                        currstate = states.turnattack;
                        AttackDelegate ad = currbattler.GetAttack(player, this);
                        StartCoroutine(ad(enemy, player, this));
                        //}
                    }
                    else
                    {
                        choiceRadial.SetActive(true);
                    }
                    currstate = states.turnattack;
                    break;
                }
            case states.turnattack:
                {
                    break;
                }
            case states.turnend:
                {
                    generalTimer += Time.deltaTime;
                    if (generalTimer >= 1.0f)
                    {
                        generalTimer = 0.0f;
                        currstate = states.turnstart;
                        battlerQueue.Add(currbattler);
                    }
                    break;
                }
            default:
                currstate = states.dead;
                break;

        }
        player.UpdateBattler();
        enemy.UpdateBattler();
        playerhp.text = player.hp.ToString();
        playermaxhp.text = player.maxhp.ToString();
        playeren.text = player.en.ToString();
        playermaxen.text = player.maxen.ToString();
    }
    void CreateDamageText(int dmg, GameObject sprite, bool critical)
    {
        GameObject dmgText = (GameObject)Instantiate(Resources.Load("Prefabs/DamageText"));
        DamageText txt = dmgText.GetComponent<DamageText>();
        txt.dmg = dmg;
        txt.critical = critical;
        Vector2 pos = sprite.transform.position -gameObject.transform.position;
        Vector2 viewportPoint = 32 * pos;
        dmgText.GetComponent<RectTransform>().anchoredPosition = viewportPoint;
        dmgText.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }
    public void DamageBattler(int dmg, Battler btl, bool critical)
    {
        btl.hp -= dmg;
       // topboard.UpdateString("The " + btl.name + " took " + dmg.ToString() + " damage!");
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
        audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Hit"));
        btl.StartFlashing(3.0f);
        btl.sprite.GetComponent<ParticleSystem>().Emit(dmg);
        CreateDamageText(dmg, btl.sprite, critical);
    }
    public void DamageBattlerNoSound(int dmg, Battler btl, bool critical)
    {
        btl.hp -= dmg;
       // topboard.UpdateString("The " + btl.name + " took " + dmg.ToString() + " damage!");
        if (btl.hp > 0)
        {
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
        btl.sprite.GetComponent<ParticleSystem>().Emit(dmg);
        CreateDamageText(dmg, btl.sprite, critical);
    }
    public void PlayerAttack()
    {
        GameObject attack = (GameObject)Instantiate(Resources.Load("Prefabs/DefaultAttackObject"));
        attack.transform.position = enemy.sprite.transform.position - new Vector3(0, .001f, 1);
        attack.GetComponent<DefaultAttackScript>().aggressor = player;
        attack.GetComponent<DefaultAttackScript>().target = enemy;
        currstate = states.turnattack;
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

    public void YieldTurn()
    {
        if (currstate == states.turnattack) { currstate = states.turnend; }
    }
    public void InitializeBattle(EnemyFactory.EnemyType typ)
    {
        GameObject playerObj = (GameObject)Instantiate(Resources.Load("Prefabs/BattleSprite"));
        playerObj.transform.parent = playerCharaCenter.transform;
        playerObj.transform.localPosition = new Vector3(1,0,0);
        player = new Battler(playerObj, "Player");
        PlayerData pd = GameObject.Find("Player").GetComponent<PlayerData>();
        player.hp = pd.stats.hp;
        player.maxhp = pd.stats.maxhp;
        player.en = pd.stats.en;
        player.maxen = pd.stats.maxen;
        player.att = pd.stats.off;
        player.def = pd.stats.def;
        player.spd = pd.stats.spd;
        player.isPlayerControlled = true;

        GameObject enemyObj = (GameObject)Instantiate(Resources.Load("Prefabs/BattleSprite"));
        enemyObj.transform.parent = enemyCharaCenter.transform;
        enemyObj.transform.localPosition = new Vector3(-1, 0, 0);
        enemy = new Battler(enemyObj, ef.CreateEnemy(typ));
        string path = enemy.GetAnimationControllerPath();
        if (path != "") 
        {
            enemyObj.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(path);
        }
        battlerQueue.Add(player);
        battlerQueue.Add(enemy);
        currstate = states.turnstart;
    }
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
    private Enemy nme = null;
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
    public Battler(GameObject sp, string name, int maxhp, int maxen, int att, int def, int spd, int lvl)
    {
        this.name = name;
        this.maxhp = hp = maxhp;
        this.maxen = en = maxen;
        this.att = att;
        this.def = def;
        this.spd = spd;
        this.lvl = lvl;
        sprite = sp;
        pos = sprite.transform.position;
    }
    public Battler(GameObject sp, Enemy enemy)
    {
        this.name = enemy.name;
        this.maxhp = hp = enemy.maxhp;
        this.maxen = en = enemy.maxen;
        this.att = enemy.att;
        this.def = enemy.def;
        this.spd = enemy.spd;
        this.lvl = enemy.lvl;
        sprite = sp;
        pos = sprite.transform.position;
        nme = enemy;
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
    public string GetAnimationControllerPath()
    {
        if (nme != null)
        {
            return nme.spritePath;
        }
        return "";
    }

    public AttackDelegate GetAttack(Battler target, BattleMasterScript bm)
    {
        if (isPlayerControlled) { Debug.Log("This shouldn't have happened! Asked player controlled battler to attack."); return null; }
        return nme.GetAttack(target, this, bm);
    }
}

public delegate IEnumerator AttackDelegate(Battler aggressor, Battler target, BattleMasterScript bm);

public class Attacks : ScriptableObject
{
    public int[] weights;
    public AttackDelegate[] attacks;
    public IEnumerator Ram(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " is charging at the " + target.name + "!");
        yield return new WaitForSeconds(2.0f);
        GameObject enemyAttack = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyAttack"));
        enemyAttack.GetComponent<AttackAnim>().aggressor = aggressor;
        enemyAttack.GetComponent<AttackAnim>().target = target;
        enemyAttack.GetComponent<AttackAnim>().at = AttackType.ram;
        enemyAttack.transform.position = target.sprite.transform.position;
    }
    public IEnumerator LoudSound(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " emits a loud sound at the " + target.name + "!");
        yield return new WaitForSeconds(2.0f);
        GameObject enemyAttack = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyAttack"));
        enemyAttack.GetComponent<AttackAnim>().aggressor = aggressor;
        enemyAttack.GetComponent<AttackAnim>().target = target;
        enemyAttack.GetComponent<AttackAnim>().at = AttackType.loudsound;
        enemyAttack.transform.position = target.sprite.transform.position;
    }
    public IEnumerator Beep(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " beeps softly.");
        yield return new WaitForSeconds(2.0f);
        bm.YieldTurn();
    }
    public Attacks()
    {
    }
    public AttackDelegate SelectAttack()
    {
        int weightsMax = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            weightsMax += weights[i];
        }
        int roll = Mathf.RoundToInt(UnityEngine.Random.Range(0.0f,1.0f) * weightsMax);
        int currindex = 0;
        while (roll > weights[currindex])
        {
            roll -= weights[currindex];
            currindex++;
        }
        return attacks[currindex];
    }
};

public class Enemy : ScriptableObject
{
    public int hp, en, maxhp, maxen, att, def, spd, lvl;
    public string name;
    public string spritePath;
    public Attacks atks;
    public Enemy(int hp,int en, int att, int def, int spd, int lvl, string name, string spritePath)
    {
        this.hp = hp;
        this.maxhp = hp;
        this.en = en;
        this.maxen = en;
        this.att = att;
        this.def = def;
        this.spd = spd;
        this.lvl = lvl;
        this.name = name;
        this.spritePath = spritePath;
    }
    public AttackDelegate GetAttack(Battler target, Battler self, BattleMasterScript bm)
    {
        return atks.SelectAttack();
    }
};
public class EnemyFactory
{
    public enum EnemyType { vroomer, lappy };
    public Enemy CreateEnemy(EnemyType ind)
    {
        switch (ind)
        {
            case EnemyType.vroomer:
            {
                Enemy nme = new Enemy(30, 10, 10, 6, 4, 1, "Spiky Vroomer", "Sprites/EnemyBattleAnims/SpikyVroomer");
                int[] weights = { 2,2 };
                Attacks atk = new Attacks();
                atk.weights = weights;
                AttackDelegate[] atks = { atk.Ram ,atk.Beep};
                atk.attacks = atks;
                nme.atks = atk;
                return nme;
                }
            case EnemyType.lappy:
                {
                    Enemy nme = new Enemy(30, 10, 10, 6, 4, 1, "Maddened Lappy", "Sprites/EnemyBattleAnims/Maddened Lappy");
                    int[] weights = { 3, 2 };
                    Attacks atk = new Attacks();
                    atk.weights = weights;
                    AttackDelegate[] atks = { atk.LoudSound, atk.Beep };
                    atk.attacks = atks;
                    nme.atks = atk;
                    return nme;
                }
            default:
            {
                Enemy nme = new Enemy(30, 8, 5, 3, 2, 1, "Spiky Vroomer", "");
                int[] weights = { 1 };
                Attacks atk = new Attacks();
                atk.weights = weights;
                AttackDelegate[] atks = { atk.Ram };
                atk.attacks = atks;
                nme.atks = atk;
                return nme;
             }
        }
    }
}
