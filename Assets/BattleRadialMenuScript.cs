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
    private bool itemMenu = false;
    private const float firstMoveTimer = 0.35f;
    private const float heldMoveTimer = 5.0f / 60.0f;
    private float currMoveTimer = 0.0f;
    private int itemSelectorIndex = 0;
    private List<ItemDirectory.ItemIndex> useableItems = new List<ItemDirectory.ItemIndex>();
    private bool held = false;
    private PlayerData pd;
    AudioSource click;
    public GameObject itemList;
    public BattleMasterScript bm;
    public Text[] itemTexts;
    public Text hpTextNum;
    public Text enTextNum;
    public RectTransform itemSelectorTransform;
    private new AudioSource audio;
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
            if (!rotating)
            {
                switch (currindex)
                {
                    case (int)RadialOptions.attack:
                        if (Input.GetKeyDown("space"))
                        {
                            bm.PlayerAttack();
                            gameObject.SetActive(false);
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
                        }
                        break;
                    case (int)RadialOptions.tactics:
                        break;
                    case (int)RadialOptions.skills:
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
                }
            }
            
            itemSelectorTransform.anchoredPosition = new Vector3(4, -(itemSelectorIndex % 4) * 24 - 12, 0);
        }
        rt.transform.eulerAngles = new Vector3(0, 0, angle);

    }
}
