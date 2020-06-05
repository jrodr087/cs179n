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
    private CameraShader cs;
    EmptyVoidCallback cbbattleend = null;
    private List<Battler> playerControlledBattlers = new List<Battler>();
    private List<Battler> enemies = new List<Battler>();
    private List<Battler> registeredBattlers = new List<Battler>();
    private int earnedEXP = 0;
    public new AudioSource audio;
    public GameObject choiceRadial;
    public Text playerhp;
    public Text playeren;
    public Text playermaxhp;
    public Text playermaxen;
    private Battler currbattler;
    private List<Battler> battlerQueue = new List<Battler>();
    private PlayerMovement movscript;
    private float generalTimer = 0.0f;
    public string songloc;
    private MusicPlayerScript mps;
    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.AddComponent<AudioSource>();
        cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
        mps = GameObject.Find("Main Camera/MusicPlayer").GetComponent<MusicPlayerScript>();
        movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
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
                    if (cbbattleend == null)
                    {
                        cs.StartWipe(EndBattle, movscript.UnlockMovement);
                    }
                    else
                    {
                        cs.StartWipe(EndBattle, cbbattleend);
                    }
                }
                break;
            case states.turnstart:
                {
                    currbattler = battlerQueue[0];
                    battlerQueue.RemoveAt(0);
                    battlerQueue.Add(currbattler);
                    currbattler.StartFlashing(1.5f);
                    if (!currbattler.isPlayerControlled)
                    {
                        //generalTimer += Time.deltaTime;
                        //if (generalTimer >= 2.0f)
                        //{
                        generalTimer = 0.0f;
                        currstate = states.turnattack;
                        AttackDelegate ad = currbattler.GetAttack(player, this);
                        StartCoroutine(ad(currbattler, player, this));
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
                    }
                    break;
                }
            default:
                currstate = states.dead;
                break;

        }
        //player.UpdateBattler();
        //enemy.UpdateBattler();
        for (int i = 0; i < registeredBattlers.Count; i++)
        {
            registeredBattlers[i].UpdateBattler();
        }
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
        Vector2 pos = sprite.transform.position - gameObject.transform.position;
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
            earnedEXP += math.max((btl.lvl * 10 - math.max((player.lvl - btl.lvl),0) * 5),0);
            battlerQueue.Remove(btl);
            playerControlledBattlers.Remove(btl);
            enemies.Remove(btl);
            if (enemies.Count < 1)
            {

                topboard.UpdateString("You won!");
                currstate = states.battlewon;
            }
            else if (playerControlledBattlers.Count < 1)
            {

                topboard.UpdateString("You lost...");
                currstate = states.dead;
            }
        }
        audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Hit"));
        //btl.StartFlashing(3.0f);
        btl.sprite.GetComponent<ParticleSystem>().Emit(dmg);
        CreateDamageText(dmg, btl.sprite, critical);
    }
    public void DamageBattlerNoSound(int dmg, Battler btl, bool critical)
    {
        btl.hp -= dmg;
       // topboard.UpdateString("The " + btl.name + " took " + dmg.ToString() + " damage!");
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
    public void BattlerAttack(AttackDelegate ad, Battler aggressor, Battler target)
    {
        currstate = states.turnattack;
        StartCoroutine(ad(aggressor, target, this));
    }
    public void PlayerAttack(Battler target)
    {
        GameObject attack = (GameObject)Instantiate(Resources.Load("Prefabs/DefaultAttackObject"));
        attack.transform.position = target.sprite.transform.position - new Vector3(0, .001f, 1);
        attack.GetComponent<DefaultAttackScript>().aggressor = player;
        attack.GetComponent<DefaultAttackScript>().target = target;
        currstate = states.turnattack;
    }
    public void EndBattle()
    {
        PlayerData pd = GameObject.Find("Player").GetComponent<PlayerData>();
        pd.stats.hp = player.hp;
        pd.stats.en = player.en;
        pd.GiveExp(earnedEXP);
        movscript.LeaveBattle();
        mps.StopSong(songloc);
    }

    public void HealBattler(int hpheal, int enheal, Battler btl)
    {
        btl.hp += hpheal;
        if (btl.hp > btl.maxhp)
        {
            btl.hp = btl.maxhp;
        }
        btl.en += enheal;
        if (btl.en > btl.maxen)
        {
            btl.en = btl.maxen;
        }
        audio.PlayOneShot((AudioClip)Resources.Load("Sounds/HealingItemUsed"));
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
        registeredBattlers.Add(player);

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
        enemies.Add(enemy);
        registeredBattlers.Add(enemy);
    }
    public void InitializeBattle(List<EnemyFactory.EnemyType> types)
    {
        GameObject playerObj = (GameObject)Instantiate(Resources.Load("Prefabs/BattleSprite"));
        playerObj.transform.parent = playerCharaCenter.transform;
        playerObj.transform.localPosition = new Vector3(1, 0, 0);
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
        playerControlledBattlers.Add(player);
        registeredBattlers.Add(player);
        battlerQueue.Add(player);
        float baseangle = 180;
        for (int i = 0; i < types.Count; i++)
        {
            GameObject enemyObj = (GameObject)Instantiate(Resources.Load("Prefabs/BattleSprite"));
            enemyObj.transform.parent = enemyCharaCenter.transform;
            float currangle = baseangle + i * (360.0f / types.Count);
            currangle = Mathf.Deg2Rad*currangle;
            enemyObj.transform.localPosition = new Vector3(1.0f * math.cos(currangle), 1.0f*math.sin(currangle), 0);
            enemy = new Battler(enemyObj, ef.CreateEnemy(types[i]));
            string path = enemy.GetAnimationControllerPath();
            if (path != "")
            {
                enemyObj.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(path);
            }
            battlerQueue.Add(enemy);
            currstate = states.turnstart;
            enemies.Add(enemy);
            registeredBattlers.Add(enemy);
        }

    }
    public void SetBattleEndCallback(EmptyVoidCallback cb)
    {
        cbbattleend = cb;
    }
    public List<Battler> GetEnemies()
    {
        return enemies;
    }

    public Battler GetCurrentBattler()
    {
        return currbattler;
    }
    public void RemoveFromRegisteredBattlers(Battler btl)
    {
        registeredBattlers.Remove(btl);
    }

    public Battler GetRandomEnemy()
    {
        int index = Mathf.RoundToInt(UnityEngine.Random.Range(0.0f,1.0f)*(enemies.Count-1));
        return enemies[index];
    }
    public Battler GetRandomEnemyIgnoringOne(Battler ignoredEnemy)
    {
        if (enemies.Count < 2) { return ignoredEnemy; } //if there isnt two enemies at least then we can only return the ignored one. oh well
        Battler temp;
        do
        {
            int index = Mathf.RoundToInt(UnityEngine.Random.Range(0.0f, (float)(enemies.Count - 1)));
            temp = enemies[index];
        } while (temp == ignoredEnemy);
        return temp;
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
    private string hurtAnimatorPath = "";
    private float hurtPercent = 0.0f;
    private bool hurtAnimation = false;
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
        if (enemy.name == "Mega Vroomer"){
            sprite.transform.localScale = new Vector3(3f, 3f, 3f);
        }
        pos = sprite.transform.position;
        nme = enemy;
        if (nme.hurtAnimation)
        {
            this.hurtAnimatorPath = nme.hurtSpritePath;
            this.hurtPercent = nme.hurtPercent;
        }
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
    
        if (!hurtAnimation && ((float)(hp)/ maxhp) <= hurtPercent && hurtAnimatorPath != "")
        {
            hurtAnimation = true;
            sprite.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(hurtAnimatorPath);
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
    public void SetHurtAnimation(string animPath,float percent)
    {

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
    public IEnumerator ShootChange(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " is gunning spare change at the " + target.name + "!");
        yield return new WaitForSeconds(2.0f);
        GameObject enemyAttack = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyAttack"));
        enemyAttack.GetComponent<AttackAnim>().aggressor = aggressor;
        enemyAttack.GetComponent<AttackAnim>().target = target;
        enemyAttack.GetComponent<AttackAnim>().at = AttackType.gunchange;
        enemyAttack.transform.position = target.sprite.transform.position;
    }
    public IEnumerator BarcodeLaser(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " is aiming its laser at the " + target.name + "!");
        yield return new WaitForSeconds(2.0f);
        GameObject enemyAttack = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyAttack"));
        enemyAttack.GetComponent<AttackAnim>().aggressor = aggressor;
        enemyAttack.GetComponent<AttackAnim>().target = target;
        enemyAttack.GetComponent<AttackAnim>().at = AttackType.barcodelaser;
        enemyAttack.transform.position = target.sprite.transform.position;
    }
    public IEnumerator ShowTargetData(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        target = bm.GetRandomEnemyIgnoringOne(aggressor);
        target.att += 1;
        topboard.UpdateString("The " + aggressor.name + " is showing target data to the " + target.name + "!" + " Their Offense went up by 1!");
        yield return new WaitForSeconds(3.0f);
        bm.YieldTurn();
    }
    public IEnumerator PlayerCombo(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        GameObject attack = (GameObject)Instantiate(Resources.Load("Prefabs/DefaultAttackObject"));
        attack.transform.position = target.sprite.transform.position - new Vector3(0, .001f, 1);
        attack.GetComponent<DefaultAttackScript>().aggressor = aggressor;
        attack.GetComponent<DefaultAttackScript>().target = target;
        attack.GetComponent<DefaultAttackScript>().at = AttackType.playerCombo;
        yield return new WaitForSeconds(1.0f/60.0f);
    }
    public IEnumerator ShowMartialArts(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        target = bm.GetRandomEnemyIgnoringOne(aggressor);
        target.def += 1;
        topboard.UpdateString("The " + aggressor.name + " is showing a martial arts video to the " + target.name + "!" + " Their Defense went up by 1!");
        yield return new WaitForSeconds(3.0f);
        bm.YieldTurn();
    }
    public IEnumerator ShowMilitaryTraining(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        target = bm.GetRandomEnemyIgnoringOne(aggressor);
        target.att += 1;
        target.def += 1;
        topboard.UpdateString("The " + aggressor.name + " is showing a military training video to the " + target.name + "!" + " Their Offense and Defense went up by 1!");
        yield return new WaitForSeconds(3.3f);
        bm.YieldTurn();
    }
    public IEnumerator DrawPower(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        aggressor.att += 1;
        aggressor.def += 1;
        topboard.UpdateString("The " + aggressor.name + " is drawing an excess of power!" + " Their Offense and Defense went up by 1!");
        yield return new WaitForSeconds(3.3f);
        bm.YieldTurn();
    }
    public IEnumerator TurnUpVolume(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        aggressor.att += 1;
        topboard.UpdateString("The " + aggressor.name + " is turning up the volume!" + " Their Offense went up by 1!");
        yield return new WaitForSeconds(3.3f);
        bm.YieldTurn();
    }
    public IEnumerator ArmsUp(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        aggressor.def += 2;
        topboard.UpdateString("The " + aggressor.name + " hides behind their arms! Their Defense has increased by 2!");
        yield return new WaitForSeconds(3.0f);
        bm.YieldTurn();
    }
    public IEnumerator GoodSide(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        int roll = Mathf.RoundToInt(UnityEngine.Random.Range(0.0f, 1.0f) * 4);
        switch (roll)
        {
            case 0:
                topboard.UpdateString("The " + aggressor.name + " looks on the bright side, supposing that their finals are probably cancelled now! They healed by 20 HP!");
                break;
            case 1:
                topboard.UpdateString("The " + aggressor.name + " looks on the bright side, realizing their schedule is looking a lot more clear now! They healed by 20 HP!");
                break;
            case 2:
                topboard.UpdateString("The " + aggressor.name + " looks on the bright side, remembering they got rid of their smart toaster after it switched to a subscription service! They healed by 20 HP!");
                break;
            case 3:
                topboard.UpdateString("The " + aggressor.name + " looks on the bright side, doubting anyone is gonna bother collecting on their student loans now! They healed by 20 HP!");
                break;
            case 4:
                topboard.UpdateString("The " + aggressor.name + " looks on the bright side, they didn't really have anything better to do tonight anyway! They healed by 20 HP!");
                break;
            default:
                topboard.UpdateString("The " + aggressor.name + " looks on the bright side, supposing that finals are probably cancelled now! They healed by 20 HP!");
                break;

        }
        aggressor.hp += 20;
        if (aggressor.hp > aggressor.maxhp)
        {
            aggressor.hp = aggressor.maxhp;
        }
        yield return new WaitForSeconds(3.5f);
        bm.YieldTurn();
    }
    public IEnumerator WindUp(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        aggressor.att += 2;
        topboard.UpdateString("The " + aggressor.name + " starts whirling their arm in a circle! Their Offense has increased by 2!");
        yield return new WaitForSeconds(3.0f);
        bm.YieldTurn();
    }
    public IEnumerator ShowUncomfortableFootage(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        target.def -= 1;
        if (target.def < 0) { target.def = 0; }
        topboard.UpdateString("The " + aggressor.name + " is showing something uncomfortable to watch to the " + target.name + "!" + " Their Defense went down by 1!");
        yield return new WaitForSeconds(3.0f);
        bm.YieldTurn();
    }
    public IEnumerator ShowExceedinglyUncomfortableFootage(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        target.def -= 2;
        if (target.def < 0) { target.def = 0; }
        topboard.UpdateString("The " + aggressor.name + " is showing something very uncomfortable to watch to the " + target.name + "!" + " Their Defense went down by 2!");
        yield return new WaitForSeconds(3.0f);
        bm.YieldTurn();
    }
    public IEnumerator WireWhip(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " is whipping its wires at the " + target.name + "!");
        yield return new WaitForSeconds(2.0f);
        GameObject enemyAttack = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyAttack"));
        enemyAttack.GetComponent<AttackAnim>().aggressor = aggressor;
        enemyAttack.GetComponent<AttackAnim>().target = target;
        enemyAttack.GetComponent<AttackAnim>().at = AttackType.gunchange;
        enemyAttack.transform.position = target.sprite.transform.position;
    }
    public IEnumerator Beep(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " beeps softly.");
        yield return new WaitForSeconds(1.5f);
        bm.YieldTurn();
    }
    public IEnumerator EjectDrawer(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " ejects its money drawer.");
        yield return new WaitForSeconds(1.5f);
        bm.YieldTurn();
    }
    public IEnumerator ScanArea(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " is scanning the area.");
        yield return new WaitForSeconds(1.5f);
        bm.YieldTurn();
    }
    public IEnumerator CallParents(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " threatens to call your parents! Mom's gonna flip!");
        yield return new WaitForSeconds(1.5f);
        bm.YieldTurn();
    }
    public IEnumerator CallPolice(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " threatens to call the police!");
        yield return new WaitForSeconds(1.5f);
        bm.YieldTurn();
    }
    public IEnumerator Vibrate(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " vibrates intensely!");
        yield return new WaitForSeconds(1.5f);
        bm.YieldTurn();
    }
    public IEnumerator DisposeBodyLookup(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " is looking up how to get rid of a body.");
        yield return new WaitForSeconds(1.5f);
        bm.YieldTurn();
    }
    public IEnumerator ActorThought(Battler aggressor, Battler target, BattleMasterScript bm)
    {
        BattleTopBoardScript topboard = GameObject.Find("Canvas/BattleTopBoard").GetComponent<BattleTopBoardScript>();
        topboard.UpdateString("The " + aggressor.name + " is wondering which actor should play them in the movie adaptation.");
        yield return new WaitForSeconds(1.5f);
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
    public bool hurtAnimation = false;
    public string hurtSpritePath;
    public float hurtPercent = 0.0f;
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
    public void SetHurtAnimation(string hurtpath,float percent)
    {
        hurtSpritePath = hurtpath;
        hurtPercent = percent;
        hurtAnimation = true;
    }
    public AttackDelegate GetAttack(Battler target, Battler self, BattleMasterScript bm)
    {
        return atks.SelectAttack();
    }
};
public class EnemyFactory
{
    public enum EnemyType { vroomer, lappy, selfphone,crashedregister,enlightenedmonitor,barcodeimprinter,tvboss, megavroomer };
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
            case EnemyType.selfphone:
                {
                    Enemy nme = new Enemy(120, 10, 18, 7, 10, 2, "Self Phone", "Sprites/EnemyBattleAnims/Self Phone");
                    int[] weights = { 2, 1, 1, 1, 1 };
                    Attacks atk = new Attacks();
                    atk.weights = weights;
                    AttackDelegate[] atks = { atk.LoudSound, atk.CallParents, atk.CallPolice, atk.Vibrate, atk.DisposeBodyLookup };
                    atk.attacks = atks;
                    nme.atks = atk;
                    return nme;
                }
            case EnemyType.crashedregister:
                {
                    Enemy nme = new Enemy(60, 10, 9, 8, 10, 2, "Crashed Register", "Sprites/EnemyBattleAnims/CrashedRegister");
                    int[] weights = { 2, 1};
                    Attacks atk = new Attacks();
                    atk.weights = weights;
                    AttackDelegate[] atks = { atk.ShootChange, atk.EjectDrawer};
                    atk.attacks = atks;
                    nme.atks = atk;
                    return nme;
                }
            case EnemyType.barcodeimprinter:
                {
                    Enemy nme = new Enemy(50, 10, 8, 8, 15, 2, "Barcode Imprinter", "Sprites/EnemyBattleAnims/BarcodeImprinter");
                    int[] weights = { 2, 1 };
                    Attacks atk = new Attacks();
                    atk.weights = weights;
                    AttackDelegate[] atks = { atk.BarcodeLaser, atk.ScanArea };
                    atk.attacks = atks;
                    nme.atks = atk;
                    return nme;
                }
            case EnemyType.enlightenedmonitor:
                {
                    Enemy nme = new Enemy(40, 10, 0, 8, 5, 2, "Enlightened Monitor", "Sprites/EnemyBattleAnims/EnlightenedMonitor");
                    int[] weights = { 2, 2, 1, 1 };
                    Attacks atk = new Attacks();
                    atk.weights = weights;
                    AttackDelegate[] atks = { atk.ShowTargetData, atk.ShowMartialArts, atk.ShowMilitaryTraining, atk.ShowUncomfortableFootage };
                    atk.attacks = atks;
                    nme.atks = atk;
                    return nme;
                }
            case EnemyType.tvboss:
                {
                    Enemy nme = new Enemy(200, 10, 12, 10, 16, 3, "Erudite TV", "Sprites/EnemyBattleAnims/TV Boss");
                    int[] weights = { 3, 1, 2, 2, 1 };
                    Attacks atk = new Attacks();
                    atk.weights = weights;
                    AttackDelegate[] atks = { atk.WireWhip, atk.ShowExceedinglyUncomfortableFootage, atk.DrawPower, atk.TurnUpVolume, atk.ActorThought };
                    atk.attacks = atks;
                    nme.atks = atk;
                    nme.SetHurtAnimation("Sprites/EnemyBattleAnims/TV Boss Injured", 0.5f);
                    return nme;
                }
            case EnemyType.megavroomer:
            {
                Enemy nme = new Enemy(100, 20, 20, 12, 8, 2, "Mega Vroomer", "Sprites/EnemyBattleAnims/SpikyVroomer");
                int[] weights = { 2,2 };
                Attacks atk = new Attacks();
                atk.weights = weights;
                AttackDelegate[] atks = { atk.Ram ,atk.Beep};
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
