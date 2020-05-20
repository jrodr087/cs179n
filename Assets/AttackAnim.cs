using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { playerDefault,enemyDefault,ram,loudsound};

public class AttackAnim : MonoBehaviour
{
    new
        // Start is called before the first frame update
        AudioSource audio;
    
    public Battler aggressor;
    public Battler target;
    public BattleMasterScript bm;
    public float percent = .5f;
    public AttackType at = AttackType.playerDefault;
    private int atknum = 0;
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
        if (at == AttackType.playerDefault)
        {
            bool crit = false;
            if (percent > 0.95f)
            {
                percent = 1.1f;
                crit = true;
            }
            int dmg = 0;
            if (atknum == 2)
            {
                dmg = (int)Mathf.Ceil(Mathf.Max((((float)aggressor.att * 1.4f - (float)target.def) * percent), 1));
                bm.DamageBattler(dmg, target, crit);
            }
            else
            {
                dmg = (int)Mathf.Ceil(Mathf.Max((((float)aggressor.att * 0.8f - (float)target.def) * percent), 1));
                bm.DamageBattlerNoSound(dmg, target, crit);
            }
            atknum++;
        }
        if (at == AttackType.enemyDefault)
        {
            int dmg = (int)Mathf.Ceil(Mathf.Max((((float)aggressor.att - (float)target.def)), 1));
            bm.DamageBattler(dmg, target, false);

        }
        if (at == AttackType.ram)
        {
            int dmg = (int)Mathf.Ceil(Mathf.Max((((float)aggressor.att *1.5f - (float)target.def)), 1));
            bm.DamageBattler(dmg, target, false);
        }
        if (at == AttackType.loudsound)
        {
            int dmg = (int)Mathf.Ceil(Mathf.Max((((float)aggressor.att * 1.2f - (float)target.def)), 1));
            bm.DamageBattler(dmg, target, false);
        }
    }
    void EndPlayerturn()
    {
        bm.YieldTurn();
        Destroyanim();
    }
    void EndEnemyTurn()
    {
        bm.YieldTurn();
        Destroyanim();
    }
    void Destroyanim()
    {
        Destroy(gameObject);
    }
}
