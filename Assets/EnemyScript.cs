using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    public PlayerMovement movscript;
    // Start is called before the first frame update
    void Start()
    {

        movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && !movscript.GetMovementLock())
        {
            Debug.Log("Enemy touched player");
            GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
            btl.transform.position = new Vector3(1000, 1000, 0);
            CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
            cs.sub = CameraSubject.battle;
            cs.Battle = btl;
            movscript.LockMovement();
            movscript.battle = btl;
            Destroy(gameObject);
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
