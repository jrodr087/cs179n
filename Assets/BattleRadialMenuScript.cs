using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRadialMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    private RectTransform rt;
    private int currindex;
    Vector2 movement;
    private const int numindices = 4;
    private float angle = 0.0f;
    private float angleinc = 90.0f;
    private bool rotating = false;
    private int direction = 0;
    private int depth = 0;
    private int enemySelectorIndex = 0;
    private bool itemMenu = false;
    private bool skillMenu = false;
    private bool enemySelectorOpen = false;
    private const float firstMoveTimer = 0.35f;
    private const float heldMoveTimer = 5.0f / 60.0f;
    private float currMoveTimer = 0.0f;
    private int itemSelectorIndex = 0;
    private int skillSelectorIndex = 0;
    private List<ItemDirectory.ItemIndex> useableItems = new List<ItemDirectory.ItemIndex>();
    private List<Skill> useableSkills;
    private bool held = false;
    private PlayerData pd;
    AudioSource click;
    public GameObject itemList;
    public GameObject skillList;
    public GameObject enemySelector;
    public GameObject optionSelector;
    public BattleMasterScript bm;
    public Text[] itemTexts;
    public Text[] skillTexts;
    public Text hpTextNum;
    public Text enTextNum;
    public Text skillEnTextNum;
    public RectTransform itemSelectorTransform;
    public RectTransform skillSelectorTransform;
    public RectTransform enemySelectorTransform;
    public Text enemySelectorNameplateText;
    public Text enemySelectorHPPlateText;
    public GameObject directionPanel;
    public Text directionText;
    public Text directionShadowText;
    private new AudioSource audio;
    private AttackDelegate at;
    private int spDeduction = 0;
    void Start()
    {
        pd = GameObject.Find("Player").GetComponent<PlayerData>();
        audio = gameObject.AddComponent<AudioSource>();
        rt = gameObject.GetComponent<RectTransform>();
        click = gameObject.GetComponent<AudioSource>();
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        List<ItemDirectory.ItemIndex> fullItemList = pd.items;
        for (int i = 0; i < fullItemList.Count; i++)
        {
            ItemDirectory.ItemIndex curritem = fullItemList[i];
            if (pd.masterItemDirectory.dir[(int)curritem].type == ItemType.consumable)
            {
                useableItems.Add(curritem);
            }
        }
    }
    public enum RadialOptions { attack,items,tactics,skills}

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        float speed = 720.0f;
        if (depth == 0)
        {
            optionSelector.SetActive(true);
            if (!rotating)
            {
                switch (currindex)
                {
                    case (int)RadialOptions.attack:
                        if (Input.GetKeyDown("space"))
                        {
                            at = null;
                            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
                            Debug.Log("opening enemy selector");
                            enemySelector.SetActive(true);
                            depth++;
                            enemySelectorOpen = true;
                            enemySelectorIndex = 0;
                            directionPanel.SetActive(true);
                            directionText.text = "Attack which enemy?";
                            directionShadowText.text = "Attack which enemy?";
                            optionSelector.SetActive(false);
                        }
                        break;
                    case (int)RadialOptions.items:
                        if (Input.GetKeyDown("space"))
                        {
                            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
                            Debug.Log("opening item list");
                            itemList.SetActive(true);
                            depth++;
                            itemMenu = true;
                            directionPanel.SetActive(true);
                            directionText.text = "Pick an item to use.";
                            directionShadowText.text = "Pick an item to use.";
                            optionSelector.SetActive(false);
                        }
                        break;
                    case (int)RadialOptions.tactics:
                        break;
                    case (int)RadialOptions.skills:
                        if (Input.GetKeyDown("space"))
                        {
                            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
                            Debug.Log("opening skill list");
                            skillList.SetActive(true);
                            depth++;
                            skillMenu = true;
                            directionPanel.SetActive(true);
                            directionText.text = "No skills, embarassing...";
                            directionShadowText.text = "No skills, embarassing...";
                            optionSelector.SetActive(false);
                        }
                        break;
                }
                if (movement.x <= -.8f)
                {
                    rotating = true;
                    angleinc = 90;
                    currindex += 1;
                    if (currindex >= numindices) { currindex = 0; }
                    direction = 1;
                    click.Play();
                }
                else if (movement.x >= .8f)
                {
                    rotating = true;
                    angleinc = -90;
                    currindex -= 1;
                    if (currindex < 0) { currindex = numindices - 1; }
                    direction = -1;
                    click.Play();
                }
            }
            else
            {
                if (direction < 0)
                {
                    angle -= Time.deltaTime * speed;
                    angleinc += Time.deltaTime * speed;
                    if (angleinc >= 0)
                    {
                        rotating = false;
                        angle = currindex * 90.0f;
                    }
                }
                if (direction > 0)
                {
                    angle += Time.deltaTime * speed;
                    angleinc -= Time.deltaTime * speed;
                    if (angleinc <= 0)
                    {
                        rotating = false;
                        angle = currindex * 90.0f;
                    }
                }
            }
        }
        else if (depth == 1 && itemMenu)
        {
            int topItemIndex = itemSelectorIndex - (itemSelectorIndex%4); //the magic number 4 here is the number of items shown at a time
            int itemNameTextIndex = 0;
            for (int i = topItemIndex; i < useableItems.Count && i < topItemIndex+4; i++) //same here with the 4
            {
                itemTexts[itemNameTextIndex].text = pd.masterItemDirectory.dir[(int)useableItems[i]].name;
                itemNameTextIndex++;
            }
            while (itemNameTextIndex < 4)
            {
                itemTexts[itemNameTextIndex].text = "";
                itemNameTextIndex++;
            }
            if (useableItems.Count == 0)
            {
                itemTexts[0].text = "No items!";
                hpTextNum.text = "N/A";
                enTextNum.text = "N/A";
            }
            else
            {
                hpTextNum.text = pd.masterItemDirectory.dir[(int)useableItems[itemSelectorIndex]].hp.ToString();
                enTextNum.text = pd.masterItemDirectory.dir[(int)useableItems[itemSelectorIndex]].en.ToString();

            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuClose"));
                Debug.Log("exiting item menu");
                itemMenu = false;
                depth--;
                itemList.SetActive(false);
                directionPanel.SetActive(false);
                return;
            }
            if (currMoveTimer >= 0.0f)
            {
                currMoveTimer -= Time.deltaTime;
            }
            if (movement.y >= .8f && currMoveTimer <= 0.0f)
            {
                audio.PlayOneShot((AudioClip)Resources.Load("Sounds/ItemSelectorMove"));
                itemSelectorIndex--;
                if (itemSelectorIndex < 0)
                {
                    if (useableItems.Count == 0)
                    {
                        itemSelectorIndex = 0;
                    }
                    else
                    {
                        itemSelectorIndex = useableItems.Count - 1;
                    }
                }
                if (!held)
                {
                    held = true;
                    currMoveTimer = firstMoveTimer;
                }
                else
                {
                    currMoveTimer = heldMoveTimer;
                }
            }
            else if (movement.y <= -.8f && currMoveTimer <= 0.0)
            {
                audio.PlayOneShot((AudioClip)Resources.Load("Sounds/ItemSelectorMove"));
                itemSelectorIndex++;
                if (itemSelectorIndex > useableItems.Count - 1)
                {
                    itemSelectorIndex = 0;
                }
                if (!held)
                {
                    held = true;
                    currMoveTimer = firstMoveTimer;
                }
                else
                {
                    currMoveTimer = heldMoveTimer;
                }

            }
            else if (movement.y > -0.8f && movement.y < 0.8f)
            {
                held = false;
                currMoveTimer = -1.0f;
            }
            
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (useableItems.Count > 0) //use the currently selected item
                {
                    Battler player = bm.GetCurrentBattler();
                    int hpheal = pd.masterItemDirectory.dir[(int)useableItems[itemSelectorIndex]].hp;
                    int enheal = pd.masterItemDirectory.dir[(int)useableItems[itemSelectorIndex]].en;
                    bm.HealBattler(hpheal, enheal, player);
                    pd.RemoveItem(useableItems[itemSelectorIndex]);
                    useableItems.RemoveAt(itemSelectorIndex);
                    itemSelectorIndex = 0;
                    depth = 0;
                    itemMenu = false;
                    itemList.SetActive(false);
                    bm.YieldTurn();
                    gameObject.SetActive(false);
                    directionPanel.SetActive(false);
                }
            }
            
            itemSelectorTransform.anchoredPosition = new Vector3(4, -(itemSelectorIndex % 4) * 24 - 12, 0);
        }
        else if (depth == 1 && enemySelectorOpen)
        {
            List<Battler> enemyList = bm.GetEnemies();
            //handle movement of the enemy selector cursor
            //the if below just lets me collapse all of the cursor movement related code super easy
            if (true)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuClose"));
                    Debug.Log("exiting enemy selector menu");
                    enemySelectorOpen = false;
                    depth--;
                    enemySelector.SetActive(false);
                    directionPanel.SetActive(false);
                    return;
                }
                if (currMoveTimer >= 0.0f)
                {
                    currMoveTimer -= Time.deltaTime;
                }
                if ((movement.y >= .8f || movement.x >= .8f)&& currMoveTimer <= 0.0f)
                {
                    audio.PlayOneShot((AudioClip)Resources.Load("Sounds/ItemSelectorMove"));
                    enemySelectorIndex--;
                    if (enemySelectorIndex < 0)
                    {
                        enemySelectorIndex = enemyList.Count - 1;
                    }
                    if (!held)
                    {
                        held = true;
                        currMoveTimer = firstMoveTimer;
                    }
                    else
                    {
                        currMoveTimer = heldMoveTimer;
                    }
                }
                else if ((movement.y <= -.8f || movement.x <= -.8f) && currMoveTimer <= 0.0)
                {
                    audio.PlayOneShot((AudioClip)Resources.Load("Sounds/ItemSelectorMove"));
                    enemySelectorIndex++;
                    if (enemySelectorIndex > enemyList.Count - 1)
                    {
                        enemySelectorIndex = 0;
                    }
                    if (!held)
                    {
                        held = true;
                        currMoveTimer = firstMoveTimer;
                    }
                    else
                    {
                        currMoveTimer = heldMoveTimer;
                    }

                }
                else if (movement.y > -0.8f && movement.y < 0.8f && movement.x < 0.8f && movement.x > -0.8f)
                {
                    held = false;
                    currMoveTimer = -1.0f;
                }
            }
            Battler currEnemy = enemyList[enemySelectorIndex];
            Vector3 distFromCenterBattle = currEnemy.sprite.transform.position - bm.gameObject.transform.position;
            enemySelectorTransform.anchoredPosition = (distFromCenterBattle * 32) + new Vector3(-16, 0, 0); //32 is the pixels per unit and 16 just offsets the selector above the enemy
            enemySelectorNameplateText.text = currEnemy.name;
            enemySelectorHPPlateText.text = currEnemy.hp.ToString();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (at == null)
                {
                    bm.PlayerAttack(currEnemy);
                    gameObject.SetActive(false);
                    enemySelector.SetActive(false);
                    directionPanel.SetActive(false);
                    depth = 0;
                    enemySelectorOpen = false;
                }
                else
                {

                    Battler player = bm.GetCurrentBattler();
                    player.en -= spDeduction;
                    bm.BattlerAttack(at, player, currEnemy);
                    gameObject.SetActive(false);
                    enemySelector.SetActive(false);
                    directionPanel.SetActive(false);
                    depth = 0;
                    enemySelectorOpen = false;
                }

            }
        }
        else if (depth == 1 && skillMenu)
        {
            useableSkills = pd.GetUseableSkills();
            int topSkillIndex = skillSelectorIndex - (skillSelectorIndex % 4); //the magic number 4 here is the number of items shown at a time
            int skillNameTextIndex = 0;
            for (int i = topSkillIndex; i < useableSkills.Count && i < topSkillIndex + 4; i++) //same here with the 4
            {
                skillTexts[skillNameTextIndex].text = useableSkills[i].name;
                skillNameTextIndex++;
            }
            while (skillNameTextIndex < 4)
            {
                skillTexts[skillNameTextIndex].text = "";
                skillNameTextIndex++;
            }
            if (useableSkills.Count == 0)
            {
                skillTexts[0].text = "No skills!";
                skillEnTextNum.text = "N/A";
            }
            else
            {
                skillEnTextNum.text = useableSkills[skillSelectorIndex].reqEn.ToString();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuClose"));
                Debug.Log("exiting skill menu");
                skillMenu = false;
                depth--;
                skillList.SetActive(false);
                directionPanel.SetActive(false);
                return;
            }
            if (currMoveTimer >= 0.0f)
            {
                currMoveTimer -= Time.deltaTime;
            }
            if (movement.y >= .8f && currMoveTimer <= 0.0f)
            {
                audio.PlayOneShot((AudioClip)Resources.Load("Sounds/ItemSelectorMove"));
                skillSelectorIndex--;
                if (skillSelectorIndex < 0)
                {
                    if (useableSkills.Count == 0)
                    {
                        skillSelectorIndex = 0;
                    }
                    else
                    {
                        skillSelectorIndex = useableSkills.Count - 1;
                    }
                }
                if (!held)
                {
                    held = true;
                    currMoveTimer = firstMoveTimer;
                }
                else
                {
                    currMoveTimer = heldMoveTimer;
                }
            }
            else if (movement.y <= -.8f && currMoveTimer <= 0.0)
            {
                audio.PlayOneShot((AudioClip)Resources.Load("Sounds/ItemSelectorMove"));
                skillSelectorIndex++;
                if (skillSelectorIndex > useableSkills.Count - 1)
                {
                    skillSelectorIndex = 0;
                }
                if (!held)
                {
                    held = true;
                    currMoveTimer = firstMoveTimer;
                }
                else
                {
                    currMoveTimer = heldMoveTimer;
                }

            }
            else if (movement.y > -0.8f && movement.y < 0.8f)
            {
                held = false;
                currMoveTimer = -1.0f;
            }
            if (useableSkills.Count > 0)
            {
                directionText.text = useableSkills[skillSelectorIndex].desc;
                directionShadowText.text = useableSkills[skillSelectorIndex].desc;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Battler player = bm.GetCurrentBattler();
                if (useableSkills.Count > 0 && useableSkills[skillSelectorIndex].reqEn <= player.en) //use the currently selected item
                {
                    if (useableSkills[skillSelectorIndex].targetsEnemy)
                    {
                        spDeduction = useableSkills[skillSelectorIndex].reqEn;
                        at = useableSkills[skillSelectorIndex].skill;
                        skillSelectorIndex = 0;
                        skillMenu = false;
                        skillList.SetActive(false);
                        audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
                        Debug.Log("opening enemy selector");
                        enemySelector.SetActive(true);
                        depth = 1;
                        enemySelectorOpen = true;
                        enemySelectorIndex = 0;
                        directionPanel.SetActive(true);
                        directionText.text = "Attack which enemy?";
                        directionShadowText.text = "Attack which enemy?";
                        optionSelector.SetActive(false);
                    }
                    else
                    {
                        player.en -= useableSkills[skillSelectorIndex].reqEn;
                        bm.BattlerAttack(useableSkills[skillSelectorIndex].skill,player, null);
                        skillSelectorIndex = 0;
                        depth = 0;
                        skillMenu = false;
                        skillList.SetActive(false);
                        gameObject.SetActive(false);
                        directionPanel.SetActive(false);
                    }
                }
            }

            skillSelectorTransform.anchoredPosition = new Vector3(4, -(skillSelectorIndex % 4) * 24 - 12, 0);
        }
        rt.transform.eulerAngles = new Vector3(0, 0, angle);

    }
}
