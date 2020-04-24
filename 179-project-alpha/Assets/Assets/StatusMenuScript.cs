using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerData pds;
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
    void Start()
    {
        stats = pds.stats;
    }

    // Update is called once per frame
    void Update()
    {
        lvl.text = stats.lvl.ToString();
        exp.text = stats.exp.ToString();
        tonext.text = stats.exptonext.ToString();
        hp.text = stats.hp.ToString();
        en.text = stats.en.ToString();
        maxhp.text = stats.maxhp.ToString();
        maxen.text = stats.maxen.ToString();
        off.text = stats.off.ToString();
        def.text = stats.def.ToString();
        spd.text = stats.spd.ToString();
    }
}
