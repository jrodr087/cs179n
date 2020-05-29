using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1EventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject swordObstacles;
    public GameObject secondRoomBlocker;
    public GameObject SecondRoomEnemy;
    public GameObject ThirdRoomEnemy;
    public TeleporterScript sideroomdoor;
    private CutsceneScript handler;
    private PlayerMovement movscript;
    void Start()
    {

        handler = GameObject.Find("/UI/VignetteController").GetComponent<CutsceneScript>();
    	movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisappearSwordObstacles()
    {
        swordObstacles.SetActive(false);
    }
    public void ReappearSwordObstacles()
    {
        swordObstacles.SetActive(true);
    }
    public void DisappearSecondRoomBlocker()
    {
        secondRoomBlocker.SetActive(false);
    }
    public void ReappearSecondRoomBlocker()
    {
        secondRoomBlocker.SetActive(true);
    }
    public void SideroomDoorLock()
    {
        sideroomdoor.locked = true;
        string[] eventtext = { "You hear the door click behind you." };
        handler.StartScene(eventtext);
    }
    public void SideroomDoorUnlock()
    {
        sideroomdoor.locked = false;
        string[] eventtext = { "You hear the door unlock behind you." };
        handler.StartScene(eventtext);
    }
    public void StartSecondRoomEnemyEvent()
    {
        string[] dialogue =
        {
            "Hey, kid.",
            "Imma hack you up."
        };
        string name = "Maddend Lappy?";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(StartSecondRoomEnemyFight);
    }
    public void EndSecondRoomEnemyEvent()
    {
        // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        movscript.UnlockMovement();
        string[] dialogue =
        {
            "Hehe, theres a lot of items for you to take...",
            "Just behind the door"
        };
        string name = "Broken Lappy";
        handler.StartDialogue(dialogue, name);
        // SideroomDoorUnlock();
    }
    public void InitializeSecondRoomEnemyFight()
    {
        // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
        btl.transform.position = new Vector3(1000, 1000, 0);
        CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        cs.sub = CameraSubject.battle;
        cs.Battle = btl;
        movscript.battle = btl;
        BattleMasterScript bm;
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.InitializeBattle(EnemyFactory.EnemyType.lappy);
        bm.SetBattleEndCallback(EndSecondRoomEnemyEvent);
        Destroy(SecondRoomEnemy);
        movscript.LockMovement();
    }
    public void StartSecondRoomEnemyFight()
    {
        CameraShader cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
        cs.StartWipe(Resources.Load<Texture>("Textures/weirdspiralwipe"), Resources.Load<Texture>("Textures/screenwipeouttex"), InitializeSecondRoomEnemyFight, null, 1.0f, 3.0f);
    }

     public void StartThirdRoomEnemyEvent()
    {
        string[] dialogue =
        {
            "I swear you leave your stuff for a second..",
            "and someone takes all your sticks",
            "GIVE IT BACK!!!"
        };
        string name = "Possesive Vromer";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(StartThirdRoomEnemyFight);
    }
    public void EndThirdRoomEnemyEvent()
    {
        // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        movscript.UnlockMovement();
        string[] dialogue =
        {
            "My Boss will here about this!"
        };
        string name = "Possesive Vromer";
        handler.StartDialogue(dialogue, name);
        // SideroomDoorUnlock();
    }
    public void InitializeThirdRoomEnemyFight()
    {
        // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
        btl.transform.position = new Vector3(1000, 1000, 0);
        CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        cs.sub = CameraSubject.battle;
        cs.Battle = btl;
        movscript.battle = btl;
        BattleMasterScript bm;
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.InitializeBattle(EnemyFactory.EnemyType.vroomer);
        bm.SetBattleEndCallback(EndThirdRoomEnemyEvent);
        Destroy(ThirdRoomEnemy);
        movscript.LockMovement();
    }
    public void StartThirdRoomEnemyFight()
    {
        CameraShader cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
        cs.StartWipe(Resources.Load<Texture>("Textures/weirdspiralwipe"), Resources.Load<Texture>("Textures/screenwipeouttex"), InitializeThirdRoomEnemyFight, null, 1.0f, 3.0f);
    }

    public void TriggerPostThirdBlockerEvent()
    {
    	string[] eventtext = { "Can't go there. Theres too many vroomers there too fight!"};
        handler.StartScene(eventtext);
        movscript.gameObject.GetComponent<Transform>().position = movscript.gameObject.GetComponent<Transform>().position + new Vector3( (float) .25,0,0);
    }

}
