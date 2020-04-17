using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerStats
{
    public int maxhp, hp, maxen, en, lvl, exp, exptonext, off,def,spd;

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
        cola, stick, hat, wallet, balloon,lighter,fan,watergun
    };

    public Item[] dir = //make sure to define a matching index in the item index enum
    {
        new Item(ItemType.consumable, "Co-Cola Cola", "An off-brand cola. Heals 10 HP.",10,0,"Items/UISprites/cocola cola"),
        new Item(ItemType.weapon, "Stick","What's sticky, round, and brown all over? Provides 3 Offense.", 0, 0, 3,0,0,"Items/UISprites/stick"),
        new Item(ItemType.accessory, "Hat", "A sports team cap. Provides 3 MaxHP and 2 Defense",3,0,0,2,0,"Items/UISprites/hat"),
        new Item(ItemType.key, "Wallet", "My wallet.",""),
        new Item(ItemType.weapon, "Balloon","Vigourously rub against your head for a portable on-demand Van de Graaff Generator. Unless you're bald, then I'm sorry. Provides 1 Offense.", 0, 0, 1,0,0, WeaponType.elec,""),
        new Item(ItemType.weapon, "Lighter","A Zippy(tm) lighter. Nice to have even if you don't smoke since it makes you look cool. Provides 2 Offense.", 0, 0, 2,0,0, WeaponType.fire,""),
        new Item(ItemType.weapon, "Handheld Fan","Not your biggest fan, but a fan nonetheless. Provides 0 Offense but 2 speed.", 0, 0, 0,0,2, WeaponType.wind,""),
        new Item(ItemType.weapon, "Water Gun","Give me a straw and a cup of water and I'll be able to dish out better water pressure than this thing. Provides 2 Offense.", 0, 0, 2,0,0, WeaponType.water,"")
    };
}


[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public PlayerStats stats = new PlayerStats();
    public List<ItemDirectory.ItemIndex> items = new List<ItemDirectory.ItemIndex>();
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
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("p"))
        {
            GiveItem(0);
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
}
