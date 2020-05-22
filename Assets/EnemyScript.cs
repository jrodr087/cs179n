using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    public PlayerMovement movscript;
    private CameraShader cs;
    private Texture inwipe;
    private Texture outwipe;
    public EnemyFactory.EnemyType type;
    // Start is called before the first frame update
    void Start()
    {

        cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
        movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        inwipe = Resources.Load<Texture>("Textures/weirdspiralwipe");
        outwipe = Resources.Load<Texture>("Textures/screenwipeouttex");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitializeBattle()
    {
        GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
        btl.transform.position = new Vector3(1000, 1000, 0);
        CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        cs.sub = CameraSubject.battle;
        cs.Battle = btl;
        movscript.battle = btl;
        BattleMasterScript bm;
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.InitializeBattle(type);
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && !movscript.GetMovementLock())
        {
            Debug.Log("Enemy touched player");
            cs.StartWipe(inwipe, outwipe, InitializeBattle, null,1.0f,3.0f);
            movscript.LockMovement();
            //SceneManager.LoadScene("BattleScene");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && !movscript.GetMovementLock())
        {
        }
    }

}
