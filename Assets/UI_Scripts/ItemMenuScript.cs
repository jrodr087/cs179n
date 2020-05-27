using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenuScript : MonoBehaviour
{
    public GameObject itemDetails;
    public Text itemName;
    public Text itemDescription;
    public Text itemEquip;
    public Image itemImage;
    public Image itemTypeImage;
    public GameObject buttonPrefab;
    public GameObject buttonContainer;

    private PlayerData pd;
    private Transform containerTransform;
    private List<GameObject> buttonList = new List<GameObject>();
    private int curItem;
    private int equippedItemsLen;
    private GameObject noItems;
    private CutsceneScript handler;

    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        handler = GameObject.Find("/UI/VignetteController").GetComponent<CutsceneScript>();
        pd = player.GetComponent<PlayerData>();
        containerTransform = buttonContainer.GetComponent<Transform>();
        itemDetails.SetActive(false);
        audio = gameObject.AddComponent<AudioSource>();
        equippedItemsLen = pd.equippedItems.Count;
        initNoItems();
    }

    private void initNoItems() {
        noItems = Instantiate(buttonPrefab) as GameObject;
        noItems.GetComponent<RectTransform>().anchoredPosition = new Vector3(10, 0, 0);
        noItems.transform.SetParent(containerTransform,false);
        noItems.transform.Find("Text").gameObject.GetComponent<Text>().text = "You have No Items :( !";
    } 

    public void UpdateButtons()
    {
        noItems.SetActive(buttonList.Count < 1);
        equippedItemsLen = pd.equippedItems.Count;
        string x = "";
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponent<Button>().GetComponent<RectTransform>().anchoredPosition = new Vector3(10, -24 * i, 0);
            buttonList[i].GetComponent<Button>().onClick.RemoveAllListeners();
            ItemDirectory.ItemIndex it = pd.items[i];
            int idx = i;
            buttonList[i].GetComponent<Button>().onClick.AddListener(() => UpdateItemInfo(idx));
            if (pd.equippedItems.Contains(i)){ x = "  <"+ (pd.equippedItems.FindIndex((int j) => j == i) + 1) +">"; }
            buttonList[i].transform.Find("Text").gameObject.GetComponent<Text>().text = pd.masterItemDirectory.dir[(int)it].name + x;
            x = "";
        }
        buttonContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonContainer.GetComponent<RectTransform>().sizeDelta.x, buttonList.Count * 24);
        if (itemDetails.activeSelf){
            UpdateItemInfo(curItem);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool changedFlag = false;
        while (buttonList.Count < pd.items.Count)
        {
            GameObject tmp = Instantiate(buttonPrefab) as GameObject;
            tmp.transform.SetParent(containerTransform,false);
            buttonList.Add(tmp);
            changedFlag = true;
        }
        while (buttonList.Count > pd.items.Count)
        {
            GameObject oldbutton = buttonList[buttonList.Count - 1];
            buttonList.RemoveAt(buttonList.Count - 1);
            Destroy(oldbutton);
            changedFlag = true;
        }
        if (equippedItemsLen != pd.equippedItems.Count){
            changedFlag = true;
        }
        if (changedFlag)
        {
            UpdateButtons();
        }           
    }

    public void UpdateItemInfo(int i)
    {
        // if (!itemDetails.activeSelf){ ToggleItemDetails(); }
        ToggleItemDetails();
        curItem = i;
        ItemDirectory.ItemIndex id = pd.items[i];
        audio.PlayOneShot((AudioClip)Resources.Load("Sounds/ItemButtonClick"));
        itemName.text = pd.masterItemDirectory.dir[(int)id].name;
        itemDescription.text = pd.masterItemDirectory.dir[(int)id].desc;
        itemImage.sprite = Resources.Load<Sprite>(pd.masterItemDirectory.dir[(int)id].iconPath);
        ItemType it = pd.masterItemDirectory.dir[(int)id].type;
        switch (it)
        {
            case ItemType.consumable:
                {
                    itemTypeImage.sprite = Resources.Load<Sprite>("Items/UISprites/item");
                    itemEquip.text = "Use?";
                    break;
                }
            case ItemType.key:
                {
                    itemTypeImage.sprite = Resources.Load<Sprite>("Items/UISprites/key");
                    itemEquip.text = "Use?";
                    break;
                }
            case ItemType.weapon:
                {
                    itemTypeImage.sprite = Resources.Load<Sprite>("Items/UISprites/attack");
                    if (pd.equippedItems.Contains(i)){
                        itemEquip.text = "Unequip?";
                    } else {
                        itemEquip.text = "Equip?";
                    }
                    break;
                }
            case ItemType.accessory:
                {
                    itemTypeImage.sprite = Resources.Load<Sprite>("Items/UISprites/accessory");
                    if (pd.equippedItems.Contains(i)){
                        itemEquip.text = "Unequip?";
                    } else {
                        itemEquip.text = "Equip?";
                    }
                    break;
                }
        }
   
        WeaponType wt = pd.masterItemDirectory.dir[(int)id].wtype;
        if (it == ItemType.weapon)
        {
            switch(wt)
            {
                case WeaponType.phys:
                    itemName.color = Color.white;
                    break;
                case WeaponType.wind:
                    itemName.color = Color.green;
                    break;
                case WeaponType.water:
                    itemName.color = Color.cyan;
                    break;
                case WeaponType.fire:
                    itemName.color = Color.red;
                    break;
                case WeaponType.elec:
                    itemName.color = Color.yellow;
                    break;
            }
        }
        else
        {
            itemName.color = Color.white;
        }
    }

    private string a;
    private string[] b = new string[1];
    public void equipItem() {
        ItemType it = pd.masterItemDirectory.dir[(int) pd.items[curItem] ].type;
        if ( it == ItemType.weapon || it == ItemType.accessory ) {
            if (pd.equippedItems.Contains(curItem) || pd.equippedItems.Count < pd.equipSlots){
                pd.equipItem(curItem);
                UpdateButtons();
            } else {
                a = "No more equip slots! Dequip an item first!";
                b[0] = a;
                handler.StartSceneFromLock(b);
                ToggleItemDetails();
            }
            
        }
        else if ( it == ItemType.consumable ) {
            pd.updateEquippedItems(curItem);
            pd.applyConsumable(pd.masterItemDirectory.dir[(int) pd.items[curItem] ]);
            a  = "Used a " + pd.masterItemDirectory.dir[(int) pd.items[curItem] ].name; 
            pd.items.RemoveAt(curItem);
            b[0] = a;
            handler.StartSceneFromLock(b);
            ToggleItemDetails();
        }
        else if ( it == ItemType.key ) {
            a = "Can't use that here!";
            b[0] = a;
            handler.StartSceneFromLock(b);
            ToggleItemDetails();
        }
        
        // UpdateItemInfo(curItem);
    }

    public void ToggleItemDetails()
    {
        if (!itemDetails.activeSelf)
        {
            itemDetails.SetActive(true);
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
        }
        else if (itemDetails.activeSelf)
        {
            itemDetails.SetActive(false);
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuClose"));
        }
    }

}
