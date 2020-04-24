using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnim : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource audio;
    public Battler aggressor;
    public Battler target;
    public BattleMasterScript bm;
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CalculateDamage()
    {
        int dmg = Mathf.Max(aggressor.att * 2 - target.def,1);
        bm.DamageBattler(dmg,target);
    }
}
