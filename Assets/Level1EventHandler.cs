using Lowscope.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Level1EventHandler : MonoBehaviour, ISaveable
{
    // Start is called before the first frame update
    public GameObject swordObstacles;
    public GameObject secondRoomBlocker;
    public GameObject SecondRoomEnemy;
    public GameObject ThirdRoomEnemy;
    public TeleporterScript sideroomdoor;
    private CutsceneScript handler;
    private PlayerMovement movscript;
    public GameObject enemyGroup;
    public GameObject[] switch1On;
    public GameObject[] switch1Off;
    public GameObject[] switch2On;
    public GameObject[] switch2Off;
    public GameObject BossTV;

    public bool sideDoorLock = false;
    public bool secondEnemyActive = true;
    public bool secondBlockerActive;
    public bool swordBlockActive;
    public bool groupEnemyActive = true;
    public bool thirdEnemyActive = true;
    public bool bossEnemyActive = true;
    

    [System.Serializable]
    public struct LOneData
    {
        public bool sideDoor;
        public bool secondEnemy;
        public bool secondBlocker;
        public bool swordBlock;
        public bool groupEnemy;
        public bool thirdEnemy;
        public bool bossEnemy;
    }

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
        swordBlockActive = false;
        swordObstacles.SetActive(swordBlockActive);
    }
    public void ReappearSwordObstacles()
    {
        swordBlockActive = true;
        swordObstacles.SetActive(swordBlockActive);
    }
    public void DisappearSecondRoomBlocker()
    {
        secondBlockerActive = false;
        secondRoomBlocker.SetActive(secondBlockerActive);
    }
    public void ReappearSecondRoomBlocker()
    {
        secondBlockerActive = true;
        secondRoomBlocker.SetActive(secondBlockerActive);
    }
    public void SideroomDoorLock()
    {
        sideDoorLock = true;
        sideroomdoor.locked = sideDoorLock;
        string[] eventtext = { "You hear the door click behind you." };
        handler.StartScene(eventtext);
    }
    public void SideroomDoorUnlock()
    {
        sideDoorLock = false;
        sideroomdoor.locked = sideDoorLock;
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
        string name = "Maddened Lappy?";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(InitializeSecondRoomEnemyFight);
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
        //Destroy(SecondRoomEnemy);
        secondEnemyActive = false;
        SecondRoomEnemy.SetActive(secondEnemyActive);
        movscript.LockMovement();
    }
    public void InitializeGroupEnemyFight()
    {
        // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
        btl.transform.position = new Vector3(1000, 1000, 0);
        CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        cs.sub = CameraSubject.battle;
        cs.Battle = btl;
        movscript.battle = btl;
        BattleMasterScript bm;
        List<EnemyFactory.EnemyType> enemies = new List<EnemyFactory.EnemyType>
        {
            EnemyFactory.EnemyType.crashedregister,
            EnemyFactory.EnemyType.barcodeimprinter,
            EnemyFactory.EnemyType.enlightenedmonitor
        };
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.InitializeBattle(enemies);
        //Destroy(enemyGroup);
        groupEnemyActive = false;
        enemyGroup.SetActive(groupEnemyActive);
    }
    public void StartEnemyGroupDrop()
    {
        movscript.LockMovement();
        enemyGroup.transform.localPosition = new Vector3(0, 15, 0);
        StartCoroutine(DropEnemyGroup());
    }
    public void OpenedTrapBox()
    {
        string[] dialogue =
        {
            "You opened the box.",
            "There seems to only be a receipt in the box, printed with the following phrase...",
            "\"S U C K E R .\""
        };
        handler.StartScene(dialogue);
        handler.SetCallback(StartEnemyGroupDrop);
    }

    private IEnumerator DropEnemyGroup()
    {
        while (enemyGroup.transform.localPosition.y > 0)
        {
            enemyGroup.transform.localPosition -= new Vector3(0, 10.0f / 60.0f, 0);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        StartGroupEnemyFight();

    }

    private void StartGroupEnemyFight()
    {
        movscript.LockMovement();
        CameraShader cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
        cs.StartWipe(Resources.Load<Texture>("Textures/weirdspiralwipe"), Resources.Load<Texture>("Textures/screenwipeouttex"), InitializeGroupEnemyFight, null, 1.0f, 3.0f);
    }
    public void StartSecondRoomEnemyFight()
    {
        movscript.LockMovement();
        CameraShader cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
        cs.StartWipe(Resources.Load<Texture>("Textures/weirdspiralwipe"), Resources.Load<Texture>("Textures/screenwipeouttex"), InitializeSecondRoomEnemyFight, null, 1.0f, 3.0f);
    }

     public void StartThirdRoomEnemyEvent()
    {
        string[] dialogue =
        {
            "I swear you leave your stuff alone for a second..",
            "...and someone takes all of your sticks and snacks...",
            "...GIVE THEM BACK!!!"
        };
        string name = "Possesive Vroomer";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(StartThirdRoomEnemyFight);
    }
    public void RemoveTV()
    {
        //Destroy(BossTV);
        bossEnemyActive = false;
        BossTV.SetActive(bossEnemyActive);
    }
    public void EndTVBossEvent()
    {
        movscript.UnlockMovement();
        string[] dialogue =
        {
            "If only my demise were televised...",
            "...If you still want a chance to make it as a main character;",
            "Find the Vroomer."
        };
        string name = "Erudite TV";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(RemoveTV);
    }
    public void InitializeTVBossFight()
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
        bm.InitializeBattle(EnemyFactory.EnemyType.tvboss);
        bm.SetBattleEndCallback(EndTVBossEvent);
        movscript.LockMovement();
    }
    public void StartTVBossFight()
    {
        movscript.LockMovement();
        CameraShader cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
        cs.StartWipe(Resources.Load<Texture>("Textures/weirdspiralwipe"), Resources.Load<Texture>("Textures/screenwipeouttex"), InitializeTVBossFight, null, 1.0f, 3.0f);
    }
    public void StartTVBossEvent()
    {
        string[] dialogue =
        {
            "I was warned you were coming.",
            "Unfortunately for you, the cue card has been clapped, the action has started, the cameras already rolling...",
            "...and you are off-screen."
        };
        string name = "Smarter TV?";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(StartTVBossFight);
    }
    public void EndThirdRoomEnemyEvent()
    {
        // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        movscript.UnlockMovement();
        string[] dialogue =
        {
            "My boss will hear about this!"
        };
        string name = "Possesive Vroomer";
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
        //Destroy(ThirdRoomEnemy);
        thirdEnemyActive = false;
        ThirdRoomEnemy.SetActive(thirdEnemyActive);
        movscript.LockMovement();
    }
    public void StartThirdRoomEnemyFight()
    {
        movscript.LockMovement();
        CameraShader cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
        cs.StartWipe(Resources.Load<Texture>("Textures/weirdspiralwipe"), Resources.Load<Texture>("Textures/screenwipeouttex"), InitializeThirdRoomEnemyFight, null, 1.0f, 3.0f);
    }

    public void TriggerPostThirdBlockerEvent()
    {
    	string[] eventtext = { "Can't go there. Theres too many vroomers there too fight!"};
        handler.StartScene(eventtext);
        movscript.gameObject.GetComponent<Transform>().position = movscript.gameObject.GetComponent<Transform>().position + new Vector3( (float) .25,0,0);
    }

    public void SwitchOneOn()
    {
        for (int i = 0; i < switch1On.Length; i++)
        {
            switch1On[i].SetActive(true);
        }
        for (int i = 0; i < switch1Off.Length; i++)
        {
            switch1Off[i].SetActive(false);
        }
    }
    public void SwitchOneOff()
    {
        for (int i = 0; i < switch1On.Length; i++)
        {
            switch1On[i].SetActive(false);
        }
        for (int i = 0; i < switch1Off.Length; i++)
        {
            switch1Off[i].SetActive(true);
        }
    }
    public void SwitchTwoOn()
    {
        for (int i = 0; i < switch2On.Length; i++)
        {
            switch2On[i].SetActive(true);
        }
        for (int i = 0; i < switch2Off.Length; i++)
        {
            switch2Off[i].SetActive(false);
        }
    }
    public void SwitchTwoOff()
    {
        for (int i = 0; i < switch2On.Length; i++)
        {
            switch2On[i].SetActive(false);
        }
        for (int i = 0; i < switch2Off.Length; i++)
        {
            switch2Off[i].SetActive(true);
        }
    }

    [SerializeField]
    private LOneData LevelOneData;

    public string OnSave()
    {
        return JsonUtility.ToJson(new LOneData() { sideDoor = sideDoorLock,
                                                   secondEnemy = secondEnemyActive,
                                                   secondBlocker = secondBlockerActive,
                                                   swordBlock = swordBlockActive,
                                                   groupEnemy = groupEnemyActive,
                                                   thirdEnemy = thirdEnemyActive,
                                                   bossEnemy = bossEnemyActive });
    }

    public void OnLoad(string data)
    {
        LevelOneData = JsonUtility.FromJson<LOneData>(data);

        //retrieve information
        sideDoorLock = LevelOneData.sideDoor;
        secondEnemyActive = LevelOneData.secondEnemy;
        secondBlockerActive = LevelOneData.secondBlocker;
        swordBlockActive = LevelOneData.swordBlock;
        groupEnemyActive = LevelOneData.groupEnemy;
        thirdEnemyActive = LevelOneData.thirdEnemy;
        bossEnemyActive = LevelOneData.bossEnemy;

        //set state
        sideroomdoor.locked = sideDoorLock;
        SecondRoomEnemy.SetActive(secondEnemyActive);
        secondRoomBlocker.SetActive(secondBlockerActive);
        swordObstacles.SetActive(swordBlockActive);
        enemyGroup.SetActive(groupEnemyActive);
        ThirdRoomEnemy.SetActive(thirdEnemyActive);
        BossTV.SetActive(bossEnemyActive);
    }

    public bool OnSaveCondition()
    {
        return true;
    }
}
