using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { playerDefault,enemyDefault};

public class AttackAnim : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource audio;
    
    public Battler aggressor;
    public Battler target;
    public BattleMasterScript bm;
    public float percent = .5f;
    public AttackType at = AttackType.playerDefault;
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
            int dmg = (int)Mathf.Ceil(Mathf.Max((((float)aggressor.att * 1.5f - (float)target.def) * percent), 1));
            bm.DamageBattler(dmg, target, crit);
        }
        if (at == AttackType.enemyDefault)
        {
            int dmg = (int)Mathf.Ceil(Mathf.Max((((float)aggressor.att - (float)target.def)), 1));
            bm.DamageBattler(dmg, target, false);

        }
    }
    void EndPlayerturn()
    {
        bm.EndPlayerAttack();
        Destroyanim();
    }
    void EndEnemyTurn()
    {
        bm.EndEnemyAttack();
        Destroyanim();
    }
    void Destroyanim()
    {
        Destroy(gameObject);
    }
}
