using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Lowscope.Saving;
using UnityEngine.SceneManagement;

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
        cola, stick, hat, wallet, balloon,lighter,fan,watergun, sword, allarnd, bitparticlegun, cheeties, gamars, gutspill, drpeppy, bsckey
    };

    public Item[] dir = //make sure to define a matching index in the item index enum
    {
        new Item(ItemType.consumable, "Co-Cola Cola", "An off-brand cola. Heals 10 HP.",10,0,"Items/UISprites/cocola cola"),
        new Item(ItemType.weapon, "Stick","What's sticky, round, and brown all over? Provides 2 Offense.", 0, 0, 2,0,0,"Items/UISprites/stick"),
        new Item(ItemType.accessory, "Hat", "A sports team cap. Provides 3 MaxHP and 2 Defense",3,0,0,2,0,"Items/UISprites/hat"),
        new Item(ItemType.key, "Wallet", "My wallet.",""),
        new Item(ItemType.weapon, "Balloon","Vigourously rub against your head for a portable on-demand Van de Graaff Generator. Provides 1 Offense.", 0, 0, 1,0,0, WeaponType.elec,""),
        new Item(ItemType.weapon, "Lighter","A Zippy(tm) lighter. Nice to have even if you don't smoke since it makes you look cool. Provides 2 Offense.", 0, 0, 2,0,0, WeaponType.fire,""),
        new Item(ItemType.weapon, "Handheld Fan","Not your biggest fan, but a fan nonetheless. Provides 0 Offense but 2 speed.", 0, 0, 0,0,2, WeaponType.wind,""),
        new Item(ItemType.weapon, "Water Gun","Give me a straw and a cup of water and I'll be able to dish out better water pressure than this thing. Provides 2 Offense.", 0, 0, 2,0,0, WeaponType.water,""),
        new Item(ItemType.weapon, "Sword","A dangerously cool sword that silently urges you on to do dangerously stupid things. Provides 20 Offense and 5 Defense.", 0, 0, 20,5,0, WeaponType.phys,"Items/UISprites/sword"),
        new Item(ItemType.weapon, "All Around Buff","Buffs all stats 10.", 10,10,10,10,10, WeaponType.phys,""),
        new Item(ItemType.weapon, "Bit Particle Gun","It's actually just a toy ray gun with a TV remote taped to it. Provides 3 Offense.", 0, 0, 3,0,0, WeaponType.phys,"Items/UISprites/bitparticlegun"),
        new Item(ItemType.consumable, "Cheeties","\"Cheeties(tm): Good for tongue; Bad for health.\" Heals 15 HP and 5 EN. Good luck getting the dust off your fingers.",15, 5,"Items/UISprites/cheeties"),
        new Item(ItemType.accessory, "GAMAR Gogglez", "Nothin gets by you in these patent pending GAMAR goggles. Will definitely improve your reaction time, guaranteed!. Provides 1 Speed.",0,0,0,0,1,"Items/UISprites/gamergoggles"),
        new Item(ItemType.consumable, "Gutsy Protein","Said to have been blessed by a legendary gym-boss. Taking it permanently increases your Offense and Defense by 1.", 0, 0, 1,1,0,"Items/UISprites/stick"),
        new Item(ItemType.consumable, "Dr. Peppy","Soft drink of choice for super geniuses (or people that pretend to be one). Restores 15 EN.",0, 15,"Items/UISprites/dr peppy"),
    };
}

public class Skill
{
    public AttackDelegate skill;
    public int reqLevel = 0;
    public int reqEn = 0;
    public string name;
    public string desc;
    public bool targetsEnemy = false;
    public Skill(AttackDelegate d, int level, int en,string name, string desc)
    {
        skill = d;
        reqLevel = level;
        reqEn = en;
        this.name = name;
        this.desc = desc;
    }
    public Skill(AttackDelegate d, int level, int en, string name, string desc, bool targ)
    {
        skill = d;
        reqLevel = level;
        reqEn = en;
        this.name = name;
        this.desc = desc;
        targetsEnemy = targ;
    }
}


//[System.Serializable]
public class PlayerData : MonoBehaviour, ISaveable
{
    public PlayerMovement movscript;
    public PlayerStats stats = new PlayerStats();
    public int equipSlots = 3;
    public List<ItemDirectory.ItemIndex> items = new List<ItemDirectory.ItemIndex>();
    public List<int> equippedItems = new List<int>();
    public ItemDirectory masterItemDirectory = new ItemDirectory();
    public List<Skill> skillList;
    public Attacks atks;
    public GameObject genderMenu;
    public bool male = true;

    [System.Serializable]
    public struct PData
    {
        public PlayerStats stat;
        public int equipSlot;
        public List<ItemDirectory.ItemIndex> item;
        public List<int> equippedItem;
        public bool gender;  //true is male and false is female
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SaveMaster.GetActiveSlot() == -1)
        {
            this.GetComponent<PlayerMovement>().LockMovement();
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
        else
        {
            this.GetComponent<PlayerMovement>().UnlockMovement();
            genderMenu.SetActive(false);
            Debug.Log("Load");
        }
        //stats.maxen = 10;
        //stats.maxhp = 20;
        //stats.hp = 20;
        //stats.en = 10;
        //stats.lvl = 1;
        //stats.exp = 0;
        //stats.exptonext = 20;
        //stats.off = 10;
        //stats.def = 8;
        //stats.spd = 6;
        //stats.pts = 5;
        atks = new Attacks();
        skillList = new List<Skill>();
        Skill armsUp = new Skill(atks.ArmsUp, 1, 2, "Arms Up", "Put your arms up in front of your much more delicate face. Defense up by 2.");
        Skill windUp = new Skill(atks.WindUp, 2, 3, "Wind Up", "Start spinning your arm around in a circle. It works in cartoons. Attack up by 2.");
        Skill heal = new Skill(atks.GoodSide, 3, 3, "Happy Thoughts", "Guess it's not all that bad. Heal yourself by 20 HP.");
        Skill combo = new Skill(atks.PlayerCombo, 3, 3, "Multi-Hit", "Uses a mix of elementary-level martial arts you learned at the age of 9 and primal instinct. Deals low damage to an enemy 3 times.", true);
        skillList.Add(armsUp);
        skillList.Add(windUp);
        skillList.Add(heal);
        skillList.Add(combo);
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("m"))
        {
            stats.pts = stats.pts + 10;
        }
        else if (Input.GetKeyDown("0"))
        {
            GiveItem((ItemDirectory.ItemIndex)0);
        }
        else if (Input.GetKeyDown("1"))
        {
            GiveItem((ItemDirectory.ItemIndex)1);
        }
        else if (Input.GetKeyDown("2"))
        {
            GiveItem((ItemDirectory.ItemIndex)2);
        }
        else if (Input.GetKeyDown("3"))
        {
            GiveItem((ItemDirectory.ItemIndex)3);
        }
        else if (Input.GetKeyDown("4"))
        {
            GiveItem((ItemDirectory.ItemIndex)4);
        }
        else if (Input.GetKeyDown("5"))
        {
            GiveItem((ItemDirectory.ItemIndex)5);
        }
        else if (Input.GetKeyDown("6"))
        {
            GiveItem((ItemDirectory.ItemIndex)6);
        }
        else if (Input.GetKeyDown("7"))
        {
            GiveItem((ItemDirectory.ItemIndex)7);
        }
        else if (Input.GetKeyDown("8"))
        {
            GiveItem((ItemDirectory.ItemIndex)8);
        }
        else if (Input.GetKeyDown("9"))
        {
            GiveItem((ItemDirectory.ItemIndex)9);
        }
        else if (Input.GetKeyDown("z"))
        {
            stats.hp = 1;
        }
    }

    public void GiveItem(ItemDirectory.ItemIndex itemIndex)
    {
        Item it = new Item();
        it = masterItemDirectory.dir[(int)itemIndex];
        items.Add(itemIndex);
        Debug.Log("Added item name: " + it.name);
    }

    public void RemoveItem(ItemDirectory.ItemIndex itemIndex)
    {
        int x = (int)items.FindIndex((ItemDirectory.ItemIndex i) => i == itemIndex);
        items.RemoveAt(x);
        if (equippedItems.Contains(x))
        {
            equippedItems.RemoveAt((int)equippedItems.FindIndex((int i) => i == x));
        }
        updateEquippedItems(x);
        Item it = masterItemDirectory.dir[(int)itemIndex];
        Debug.Log("Removed item name: " + it.name);
    }

    public void modifyStats(string stat, int change)
    {
        Debug.Log("Modifying stats");
        // 

        switch (stat)
        {
            case "maxen":
                stats.maxen = stats.maxen + change;
                break;
            case "maxhp":
                stats.maxhp = stats.maxhp + change;
                break;
            case "off":
                stats.off = stats.off + change;
                break;
            case "def":
                stats.def = stats.def + change;
                break;
            case "spd":
                stats.spd = stats.spd + change;
                break;
            case "pts":
                stats.pts = stats.pts + change;
                break;
            default: break;
        }

        Debug.Log("Modified Stat: " + stat + " by " + change);
    }

    public void equipItem(int curItem)
    {
        if (equippedItems.Contains(curItem))
        {
            equippedItems.RemoveAt((int)equippedItems.FindIndex((int i) => i == curItem));
            applyItem(masterItemDirectory.dir[(int)items[curItem]], false);
            Debug.Log("Dequipped item " + masterItemDirectory.dir[(int)items[curItem]].name);
        }
        else if (equippedItems.Count < equipSlots)
        {
            equippedItems.Add(curItem);
            applyItem(masterItemDirectory.dir[(int)items[curItem]], true);
            Debug.Log("Equipped item " + masterItemDirectory.dir[(int)items[curItem]].name);
        }
    }

    public void applyItem(Item i, bool apply)
    {
        if (apply)
        {
            stats.maxhp += i.hp;
            stats.maxen += i.en;
            stats.hp += i.hp;
            stats.en += i.en;
            stats.off += i.off;
            stats.def += i.def;
            stats.spd += i.spd;
        }
        else
        {
            stats.maxhp -= i.hp;
            stats.maxen -= i.en;
            if (stats.hp > stats.maxhp)
            {
                stats.hp = stats.maxhp;
            }
            if (stats.en > stats.maxen)
            {
                stats.en = stats.maxen;
            }
            stats.off -= i.off;
            stats.def -= i.def;
            stats.spd -= i.spd;
        }
    }

    public void applyConsumable(Item i)
    {
        stats.hp += i.hp;
        stats.en += i.en;
        if (stats.hp > stats.maxhp)
        {
            stats.hp = stats.maxhp;
        }
        if (stats.en > stats.maxen)
        {
            stats.en = stats.maxen;
        }
        stats.off += i.off;
        stats.def += i.def;
    }

    public void GiveExp(int exp)
    {
        stats.exp += exp;
        while (stats.exp >= stats.exptonext)
        {
            stats.exp = stats.exp - stats.exptonext;
            stats.lvl++;
            stats.exptonext = Mathf.RoundToInt(Mathf.Pow((stats.lvl * 20), 1.05f));
            stats.pts = stats.pts + 5;
            gameObject.GetComponent<PlayerMovement>().levelled = true;
        }
    }

    public void updateEquippedItems(int it)
    {
        for (int i = 0; i < equippedItems.Count; i++)
        {
            if (equippedItems[i] > it)
            {
                equippedItems[i] = equippedItems[i] - 1;
            }
        }
    }
    
    public void setGender(bool g)
    {
        if (g)
            male = true;
        else
            male = false;
    }

    [SerializeField]
    private PData playData;
    public string OnSave()
    {
        //string stat = JsonUtility.ToJson(stats);
        return JsonUtility.ToJson(new PData() { stat = this.stats, 
                                                equipSlot = this.equipSlots, 
                                                item = this.items, 
                                                equippedItem = this.equippedItems,
                                                gender = this.male});
    }

    public void OnLoad(string data)
    {
        //stats = JsonUtility.FromJson<PlayerStats>(data);
        playData = JsonUtility.FromJson<PData>(data);
        
        stats = playData.stat;
        equipSlots = playData.equipSlot;
        items = playData.item;
        equippedItems = playData.equippedItem;
        male = playData.gender;
        if (male)
        {
            //TO DO : change to male sprite
        }
        else
        {
            //TO DO: change to female sprite
        }
    }

    public bool OnSaveCondition()
    {
        return true;
    }

    public List<Skill> GetUseableSkills()
    {
        List<Skill> useableSkills = new List<Skill>();
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].reqLevel <= stats.lvl)
            {
                useableSkills.Add(skillList[i]);
            }
        }
        return useableSkills;
    }
}