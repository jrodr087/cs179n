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
    private float t = 0.0f;
    private float radius = 1.2f;
    AudioSource audio;

    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        audio.loop = true;
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        t = t + Time.deltaTime;
        float currradius = (Mathf.Sin(t*1.4f)) * radius;
        float y = Mathf.Sin(t * 3.4f) *currradius;
        float x = Mathf.Cos(t * 3.4f) * currradius;
        crosshair1.transform.position = new Vector3(x, y, 0);
        crosshair2.transform.position = new Vector3(-x, -y, 0);
        if (Input.GetKeyDown("space"))
        {
            GameObject anim = (GameObject)Instantiate(Resources.Load("Prefabs/ComboAttackEffect"));
            anim.transform.position = gameObject.transform.position;
            AttackAnim attack = anim.GetComponent<AttackAnim>();
            attack.aggressor = aggressor;
            attack.target = target;
            Destroy(gameObject);
        }
    }
}
