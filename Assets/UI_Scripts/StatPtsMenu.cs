using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPtsMenu : MonoBehaviour
{
	public PlayerData pds;
    public PlayerStats stats;
    public Text maxhp;
    public Text maxen;
    public Text off;
    public Text def;
    public Text spd;
    public Text pts;

    private int ptsint;
    private int maxhpint;
    private int maxenint;
    private int offint;
    private int defint;
    private int spdint;

    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        pds = GameObject.Find("Player").GetComponent<PlayerData>();
        stats = pds.stats;

        ptsint = stats.pts;
    	maxhpint = 0;
    	maxenint = 0;
    	offint = 0;
    	defint = 0;
    	spdint = 0;
        audio = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    	ptsint = stats.pts - maxhpint - maxenint - offint - defint - spdint;
        maxhp.text = "MaxHp + " + maxhpint + " = " + (stats.maxhp + maxhpint).ToString();
        maxen.text = "MaxEn + " + maxenint + " = " + (stats.maxen + maxenint).ToString();
        off.text = "Offense + " + offint + " = " + (stats.off + offint).ToString();
        def.text = "Defense + " + defint + " = " + (stats.def + defint).ToString();
        spd.text = "Speed + " + spdint + " = " + (stats.spd + spdint).ToString();
        pts.text = "Points Left: " + ptsint.ToString();
    }

    public void clickmaxhp() {
    	if (ptsint > 0){
    		ptsint--;
    		maxhpint++;
    	}
    }

    public void clickmaxen() {
    	if (ptsint > 0){
    		ptsint--;
    		maxenint++;
    	}
    }

    public void clickoff() {
    	if (ptsint > 0){
    		ptsint--;
    		offint++;
    	}
    }

    public void clickdef() {
    	if (ptsint > 0){
    		ptsint--;
    		defint++;
    	}
    }

    public void clickspd() {
    	if (ptsint > 0){
    		ptsint--;
    		spdint++;
    	}
    }

    public void clicksave() {
    	pds.modifyStats("maxhp", maxhpint);
    	pds.modifyStats("maxen", maxenint);
    	pds.modifyStats("off", offint);
    	pds.modifyStats("def", defint);
    	pds.modifyStats("spd", spdint);
    	pds.modifyStats("pts", ptsint-stats.pts);

    	maxhpint = 0;
    	maxenint = 0;
    	offint = 0;
    	defint = 0;
    	spdint = 0;
        audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
    }
}
