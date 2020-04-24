using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenuScript : MonoBehaviour
{
    public Text itemName;
    public Text itemDescription;
    public Image itemImage;
    public Image itemTypeImage;
    public GameObject buttonPrefab;
    public GameObject buttonContainer;
    private PlayerData pd;
    private Transform containerTransform;
    AudioSource audio;
    private List<GameObject> buttonList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        pd = player.GetComponent<PlayerData>();
        containerTransform = buttonContainer.GetComponent<Transform>();
        audio = gameObject.AddComponent<AudioSource>();
    }

    void UpdateButtons()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponent<Button>().GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -24 * i, 0);
            buttonList[i].GetComponent<Button>().onClick.RemoveAllListeners();
            ItemDirectory.ItemIndex it = pd.items[i];
            buttonList[i].GetComponent<Button>().onClick.AddListener(() => UpdateItemInfo(it));
            buttonList[i].transform.Find("Text").gameObject.GetComponent<Text>().text = pd.masterItemDirectory.dir[(int)it].name;
        }
        buttonContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonContainer.GetComponent<RectTransform>().sizeDelta.x, buttonList.Count * 24);
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
        if (changedFlag)
        {
            UpdateButtons();
        }
           
    }

    public void UpdateItemInfo(ItemDirectory.ItemIndex id)
    {
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
                    break;
                }
            case ItemType.key:
                {
                    itemTypeImage.sprite = Resources.Load<Sprite>("Items/UISprites/key");
                    break;
                }
            case ItemType.weapon:
                {
                    itemTypeImage.sprite = Resources.Load<Sprite>("Items/UISprites/attack");
                    break;
                }
            case ItemType.accessory:
                {
                    itemTypeImage.sprite = Resources.Load<Sprite>("Items/UISprites/accessory");
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

}
