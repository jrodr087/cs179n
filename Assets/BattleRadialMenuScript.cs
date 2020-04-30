using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRadialMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    private RectTransform rt;
    private int currindex;
    Vector2 movement;
    private const int numindices = 4;
    private float angle = 0.0f;
    private float angleinc = 90.0f;
    private bool rotating = false;
    private int direction = 0;
    AudioSource click;
    public BattleMasterScript bm;
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        click = gameObject.GetComponent<AudioSource>();
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
    }
    public enum RadialOptions { attack,items,tactics,skills}

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        float speed = 720.0f;
        if (!rotating)
        {
            switch (currindex)
            {
                case (int)RadialOptions.attack:
                    if (Input.GetKeyDown("space"))
                    {
                        Debug.Log("test");
                        bm.PlayerAttack();
                        gameObject.SetActive(false);
                    }
                    break;
                case (int)RadialOptions.items:
                    break;
                case (int)RadialOptions.tactics:
                    break;
                case (int)RadialOptions.skills:
                    break;
            }
            if (movement.x <= -.8f)
            {
                rotating = true;
                angleinc = 90;
                currindex += 1;
                if (currindex >= numindices) { currindex = 0; }
                direction = 1;
                click.Play();
            }
            else if (movement.x >= .8f)
            {
                rotating = true;
                angleinc = -90;
                currindex -= 1;
                if (currindex < 0) { currindex = numindices - 1; }
                direction = -1;
                click.Play();
            }
        }
        else
        {
            if (direction < 0)
            {
                angle -= Time.deltaTime * speed;
                angleinc += Time.deltaTime * speed;
                if (angleinc >= 0)
                {
                    rotating = false;
                    angle = currindex * 90.0f;
                }
            }
            if (direction > 0)
            {
                angle += Time.deltaTime * speed;
                angleinc -= Time.deltaTime * speed;
                if (angleinc <= 0)
                {
                    rotating = false;
                    angle = currindex * 90.0f;
                }
            }
        }
        rt.transform.eulerAngles = new Vector3(0, 0, angle);

    }
}
