using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipMenuScript : MonoBehaviour
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
    AudioSource audio;
    private List<GameObject> buttonList = new List<GameObject>();
    private List<GameObject> emptySlots = new List<GameObject>();
    private int curItem;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        pd = player.GetComponent<PlayerData>();
        containerTransform = buttonContainer.GetComponent<Transform>();
        itemDetails.SetActive(false);
        audio = gameObject.AddComponent<AudioSource>();
        EmptySlotsInit();
    }

    void EmptySlotsInit() {
        for (int i = 0; i < pd.equipSlots; i++){
        	GameObject tmp = Instantiate(buttonPrefab) as GameObject;
            tmp.transform.SetParent(containerTransform,false);
            emptySlots.Add(tmp);
            tmp.GetComponent<Button>().GetComponent<RectTransform>().anchoredPosition = new Vector3(10, (-30 * i) - 20, 0);
            tmp.transform.Find("Name").gameObject.GetComponent<Text>().text = (i+ 1) + ". Slot Empty";
            tmp.transform.Find("HpText").gameObject.GetComponent<Text>().text = "0";
            tmp.transform.Find("EnText").gameObject.GetComponent<Text>().text = "0";
            tmp.transform.Find("OffText").gameObject.GetComponent<Text>().text = "0";
            tmp.transform.Find("DefText").gameObject.GetComponent<Text>().text = "0";
            tmp.transform.Find("SpdText").gameObject.GetComponent<Text>().text = "0";
        }
    }
    void UpdateButtons()
    {
    	int i = 0;
        for (i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponent<Button>().GetComponent<RectTransform>().anchoredPosition = new Vector3(10, (-30 * i) - 20, 0);
            buttonList[i].GetComponent<Button>().onClick.RemoveAllListeners();
            ItemDirectory.ItemIndex it = pd.items[pd.equippedItems[i]];
            int idx = pd.equippedItems[i];
            buttonList[i].GetComponent<Button>().onClick.AddListener(() => UpdateItemInfo(idx));
            buttonList[i].transform.Find("Name").gameObject.GetComponent<Text>().text = (i+ 1) + ". " + pd.masterItemDirectory.dir[(int)it].name;
            buttonList[i].transform.Find("HpText").gameObject.GetComponent<Text>().text = pd.masterItemDirectory.dir[(int)it].hp.ToString();
            buttonList[i].transform.Find("EnText").gameObject.GetComponent<Text>().text = pd.masterItemDirectory.dir[(int)it].en.ToString();
            buttonList[i].transform.Find("OffText").gameObject.GetComponent<Text>().text = pd.masterItemDirectory.dir[(int)it].off.ToString();
            buttonList[i].transform.Find("DefText").gameObject.GetComponent<Text>().text = pd.masterItemDirectory.dir[(int)it].def.ToString();
            buttonList[i].transform.Find("SpdText").gameObject.GetComponent<Text>().text = pd.masterItemDirectory.dir[(int)it].spd.ToString();
        	emptySlots[i].SetActive(false);
        }
        for (; i < pd.equipSlots; i++) {
        	emptySlots[i].SetActive(true);
        }
        buttonContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonContainer.GetComponent<RectTransform>().sizeDelta.x, (pd.equipSlots * 30) + 20);
    }

    // Update is called once per frame
    void Update()
    {
        bool changedFlag = false;
        while (buttonList.Count < pd.equippedItems.Count)
        {
            GameObject tmp = Instantiate(buttonPrefab) as GameObject;
            tmp.transform.SetParent(containerTransform,false);
            buttonList.Add(tmp);
            changedFlag = true;
        }
        while (buttonList.Count > pd.equippedItems.Count)
        {
            GameObject oldbutton = buttonList[buttonList.Count - 1];
            buttonList.RemoveAt(buttonList.Count - 1);
            Destroy(oldbutton);
            changedFlag = true;
        }
        if (changedFlag)
        {
            UpdateButtons();
        }           
    }

    public void UpdateItemInfo(int i)
    {
    	ItemDirectory.ItemIndex id = pd.items[i];
    	if (!itemDetails.activeSelf){ ToggleItemDetails(); }
        curItem = i;
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

    public void equipItem() {
        pd.equipItem(curItem);
        ToggleItemDetails(); 	
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
