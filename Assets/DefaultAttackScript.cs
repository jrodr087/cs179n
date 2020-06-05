using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefaultAttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject crosshair1;
    public GameObject crosshair2;
    public Battler aggressor;
    public Battler target;
    public AttackType at = AttackType.playerDefault;
    private float t = 0.0f;
    private float radius = 1.2f;
    AudioSource audio;

    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        audio.loop = true;
        audio.Play();
        t = Random.Range(0.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        t = t + Time.deltaTime*(Mathf.Max(1+(target.spd-aggressor.spd)/10.0f,1.0f));
        float unitradius = (Mathf.Sin(t * 1.4f));
        float currradius = unitradius * radius;
        float y = Mathf.Sin(t * 3.4f) *currradius;
        float x = Mathf.Cos(t * 3.4f) * currradius;
        crosshair1.transform.localPosition = new Vector3(x, y, 0);
        crosshair2.transform.localPosition = new Vector3(-x, -y, 0);
        if (Input.GetKeyDown("space"))
        {
            float percent = 1.0f - Mathf.Abs(unitradius);
            GameObject anim;
            if (at == AttackType.playerCombo)
            {
                anim = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyAttack"));
                anim.GetComponent<AudioSource>().clip = (AudioClip)Instantiate(Resources.Load("Sounds/AttackSounds/Combo"));
                anim.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)Instantiate(Resources.Load("Sprites/AttackAnimations/ComboAnim"));
            }
            else
            {
                anim = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyAttack"));
            }
            anim.transform.position = gameObject.transform.position;
            AttackAnim attack = anim.GetComponent<AttackAnim>();
            attack.aggressor = aggressor;
            attack.target = target;
            attack.percent = percent;
            attack.at = at;
            Destroy(gameObject);
        }
    }
}
