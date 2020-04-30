using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public class PlayerStats
{
    public int maxhp, hp, maxen, en, lvl, exp, exptonext, off,def,spd,pts;

    public PlayerStats()
    {

    }
}

[System.Serializable]
public enum ItemType { consumable,key,weapon,accessory}
[System.Serializable]
public enum WeaponType { phys,wind,fire,water,elec}
[System.Serializable]
public class Item
{
    public ItemType type;
    public string name;
    public string desc;
    public string iconPath;
    public int hp, en, off, def, spd;
    public WeaponType wtype = WeaponType.phys;
    public Item(ItemType t,string nm,string dsc, string imagePath) //this should be used for key items
    {
        type = t;
        name = nm;
        desc = dsc;
        hp = 0;
        en = 0;
        off = 0;
        def = 0;
        spd = 0;
        if (imagePath == "")
        {
            iconPath = "Items/UISprites/placeholder";
        }
        else
        {
            iconPath = imagePath;
        }
    }
    public Item() //this constructor should never be used but is here just in case
    {
        type = ItemType.consumable;
        hp = 0;
        en = 0;
        off = 0;
        def = 0;
        spd = 0;
        name = "Bugged Item";
        desc = "You shouldn't see this";
        iconPath = "Items/UISprites/placeholder";
    }

    public Item(ItemType t, string nm, string dsc, int hp, int en, string imagePath)//this constructor should be used mainly for consumables.
    { //the player will regain the specified hp and en when using the item
        type = t;
       
        this.hp = hp;
        this.en = en;
        off = 0;
        def = 0;
        spd = 0;
        name = nm;
        desc = dsc;
        if (imagePath == "")
        {
            iconPath = "Items/UISprites/placeholder";
        }
        else
        {
            iconPath = imagePath;
        }
    }
    public Item(ItemType t, string nm, string dsc, int hp, int en, int off, int def, int spd, string imagePath)//this constructor should be used mainly for equippable items like weapons and accesories.
    {
        //stats defined for equippables will increase the player's stats by those values while equipped
        //a nonzero hp/en value will increase their max hp/en
        type = t; 
        this.hp = hp;
        this.en = en;
        this.off = off;
        this.def = def;
        this.spd = spd;
        name = nm;
        desc = dsc;
        if (imagePath == "")
        {
            iconPath = "Items/UISprites/placeholder";
        }
        else
        {
            iconPath = imagePath;
        }
    }
    public Item(ItemType t, string nm, string dsc, int hp, int en, int off, int def, int spd, WeaponType wt, string imagePath)//this constructor should be used for non-physical typed weapons
    {
        //stats defined for equippables will increase the player's stats by those values while equipped
        //a nonzero hp/en value will increase their max hp/en
        type = t;
        this.hp = hp;
        this.en = en;
        this.off = off;
        this.def = def;
        this.spd = spd;
        name = nm;
        desc = dsc;
        wtype = wt;
        if (imagePath == "")
        {
            iconPath = "Items/UISprites/placeholder";
        }
        else
        {
            iconPath = imagePath;
        }
    }

}
//[System.Serializable]
public class ItemDirectory
{
    //define items here by adding a new item to the directory array and the itemindex enum
    //make sure you only add to the end of each to make sure they match.


    public enum ItemIndex //make sure to define a matching item in the item array dir
    {
        cola, stick, hat, wallet, balloon,lighter,fan,watergun, sword, allarnd
    };

    public Item[] dir = //make sure to define a matching index in the item index enum
    {
        new Item(ItemType.consumable, "Co-Cola Cola", "An off-brand cola. Heals 10 HP.",10,0,"Items/UISprites/cocola cola"),
        new Item(ItemType.weapon, "Stick","What's sticky, round, and brown all over? Provides 3 Offense.", 0, 0, 3,0,0,"Items/UISprites/stick"),
        new Item(ItemType.accessory, "Hat", "A sports team cap. Provides 3 MaxHP and 2 Defense",3,0,0,2,0,"Items/UISprites/hat"),
        new Item(ItemType.key, "Wallet", "My wallet.",""),
        new Item(ItemType.weapon, "Balloon","Vigourously rub against your head for a portable on-demand Van de Graaff Generator. Provides 1 Offense.", 0, 0, 1,0,0, WeaponType.elec,""),
        new Item(ItemType.weapon, "Lighter","A Zippy(tm) lighter. Nice to have even if you don't smoke since it makes you look cool. Provides 2 Offense.", 0, 0, 2,0,0, WeaponType.fire,""),
        new Item(ItemType.weapon, "Handheld Fan","Not your biggest fan, but a fan nonetheless. Provides 0 Offense but 2 speed.", 0, 0, 0,0,2, WeaponType.wind,""),
        new Item(ItemType.weapon, "Water Gun","Give me a straw and a cup of water and I'll be able to dish out better water pressure than this thing. Provides 2 Offense.", 0, 0, 2,0,0, WeaponType.water,""),
        new Item(ItemType.weapon, "Sword","A cool sword. Provides 20 Offense and 5 Defense.", 0, 0, 20,5,0, WeaponType.phys,"Items/UISprites/sword"),
        new Item(ItemType.weapon, "All Around Buff","Buffs all stats 10.", 10,10, 10,10,10, WeaponType.phys,"")
    };
}


[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public PlayerStats stats = new PlayerStats();
    public int equipSlots = 3;
    public List<ItemDirectory.ItemIndex> items = new List<ItemDirectory.ItemIndex>();
    public List<int> equippedItems = new List<int>();
    public ItemDirectory masterItemDirectory = new ItemDirectory();
    // Start is called before the first frame update
    void Start()
    {
        stats.maxen = 10;
        stats.maxhp = 20;
        stats.hp = 15;
        stats.en = 5;
        stats.lvl = 1;
        stats.exp = 0;
        stats.exptonext = 20;
        stats.off = 10;
        stats.def = 8;
        stats.spd = 6;
        stats.pts = 5;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("m"))
        {
            stats.pts = stats.pts+10;
        }
        else if (Input.GetKeyDown("0"))
        {
            GiveItem((ItemDirectory.ItemIndex) 0);
        }
        else if (Input.GetKeyDown("1"))
        {
            GiveItem((ItemDirectory.ItemIndex) 1);
        }
        else if (Input.GetKeyDown("2"))
        {
            GiveItem((ItemDirectory.ItemIndex) 2);
        }
        else if (Input.GetKeyDown("3"))
        {
            GiveItem((ItemDirectory.ItemIndex) 3);
        }
        else if (Input.GetKeyDown("4"))
        {
            GiveItem((ItemDirectory.ItemIndex) 4);
        }
        else if (Input.GetKeyDown("5"))
        {
            GiveItem((ItemDirectory.ItemIndex) 5);
        }
        else if (Input.GetKeyDown("6"))
        {
            GiveItem((ItemDirectory.ItemIndex) 6);
        }
        else if (Input.GetKeyDown("7"))
        {
            GiveItem((ItemDirectory.ItemIndex) 7);
        }
        else if (Input.GetKeyDown("8"))
        {
            GiveItem((ItemDirectory.ItemIndex) 8);
        }
        else if (Input.GetKeyDown("9"))
        {
            GiveItem((ItemDirectory.ItemIndex) 9);
        }
    }

    public void GiveItem(ItemDirectory.ItemIndex itemIndex)
    {
        //ItemType type = masterItemDirectory.dir[itemIndex].type;
        //switch (type)
        //{
        //    case ItemType.consumable:
        //    {
        //            Consumable it = new Consumable();
        //            it = masterItemDirectory.dir[itemIndex];
        //            items.Add(it);
        //            Debug.Log("Added item name: " + it.name);
        //            break;
        //    }
        //}
        Item it = new Item();
        it = masterItemDirectory.dir[(int)itemIndex];
        items.Add(itemIndex);
        Debug.Log("Added item name: " + it.name);
    }

    public void modifyStats(string stat, int change) {
        Debug.Log("Modifying stats");
        // 

        switch (stat) {
            case "maxen": stats.maxen = stats.maxen + change;
                break;
            case "maxhp": stats.maxhp = stats.maxhp + change;
                break;
            case "off": stats.off = stats.off + change;
                break;
            case "def": stats.def = stats.def + change;
                break;
            case "spd": stats.spd = stats.spd + change;
                break;
            case "pts": stats.pts = stats.pts + change;
                break;
            default: break;
        }

        Debug.Log("Modified Stat: " + stat + " by " + change);
    }

    public void equipItem (int curItem){
        if (equippedItems.Contains(curItem)){
            equippedItems.RemoveAt( (int) equippedItems.FindIndex( (int i) => i == curItem) );
            applyItem(masterItemDirectory.dir[(int)items[curItem]], false);
            Debug.Log("Dequipped item " + masterItemDirectory.dir[(int)items[curItem]].name);
        } 
        else if (equippedItems.Count < 3){
            equippedItems.Add(curItem);
            applyItem(masterItemDirectory.dir[(int)items[curItem]], true);
            Debug.Log("Equipped item " + masterItemDirectory.dir[(int)items[curItem]].name);
        }
    }

    private void applyItem(Item i, bool apply) {
        if (apply){
            stats.hp += i.hp;
            stats.en += i.en;
            stats.off += i.off;
            stats.def += i.def;
            stats.spd += i.spd;
        } else {
            stats.hp -= i.hp;
            stats.en -= i.en;
            stats.off -= i.off;
            stats.def -= i.def;
            stats.spd -= i.spd;
        }
    }
}
