using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusMenuScript : MonoBehaviour
{
    public GameObject statPtsMenu;
    // Start is called before the first frame update
    public PlayerData pd;
    public PlayerStats stats;
    public Text lvl;
    public Text exp;
    public Text tonext;
    public Text hp;
    public Text en;
    public Text maxhp;
    public Text maxen;
    public Text off;
    public Text def;
    public Text spd;
    public Text pts;
    public GameObject ptsButton;

    AudioSource audio;

    void Start()
    {
        pd = GameObject.Find("Player").GetComponent<PlayerData>();
        stats = pd.stats;
        statPtsMenu.SetActive(false);
        audio = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        (int itmhp, int itmen, int itmoff, int itmdef, int itmspd) = getItemStats();

        lvl.text = stats.lvl.ToString();
        exp.text = stats.exp.ToString();
        tonext.text = stats.exptonext.ToString();
        hp.text = stats.hp.ToString();
        en.text = stats.en.ToString();
        maxhp.text = stats.maxhp.ToString() + " (+" + itmhp + ")";
        maxen.text = stats.maxen.ToString() + " (+" + itmen + ")";
        off.text = stats.off.ToString() + " (+" + itmoff + ")";
        def.text = stats.def.ToString() + " (+" + itmdef + ")";
        spd.text = stats.spd.ToString() + " (+" + itmspd + ")";
        if (stats.pts < 1 && !statPtsMenu.activeSelf){
            ptsButton.SetActive(false);
        }
        else if (stats.pts < 1) {
            pts.text = "Close Stat Points Menu";
        }
        else {
        	ptsButton.SetActive(true);
            pts.text = "You have Stat Points! \nAllocate Stats?";
        }
        
    }

    public (int,int,int,int,int) getItemStats() {
        int a = 0 ,b = 0,c = 0,d = 0,e = 0;
        Item tmp;
        foreach ( int i in pd.equippedItems ){
            tmp = pd.masterItemDirectory.dir[(int)pd.items[i]];
            a += tmp.hp;
            b += tmp.en;
            c += tmp.off;
            d += tmp.def;
            e += tmp.spd;
        }
        return (a,b,c,d,e);
    }

    public void ToggleStatPointsMenu()
    {
        if (!statPtsMenu.activeSelf)
        {
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
            statPtsMenu.SetActive(true);
        }
        else if (statPtsMenu.activeSelf)
        {
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuClose"));
            statPtsMenu.SetActive(false);
        }
    }
}
