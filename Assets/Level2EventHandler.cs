using Lowscope.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Level2EventHandler : MonoBehaviour, ISaveable
{

    // public MusicPlayerScript mps;
    
    private CutsceneScript handler;
    private PlayerMovement movscript;

    public GameObject M0;
    public bool M0Active = true;

    public GameObject M1;
    public bool M1Active = true;

    public GameObject M2;
    public bool M2Active = true;

    public GameObject M3;
    public bool M3Active = true;

    public GameObject M4;
    public bool M4Active = true;

    public GameObject M5;
    public bool M5Active = true;

    private BattleMasterScript bm;

    // Start is called before the first frame update
    // public GameObject swordObstacles;
    // public GameObject secondRoomBlocker;
    // public GameObject SecondRoomEnemy;
    // public GameObject ThirdRoomEnemy;
    // public TeleporterScript sideroomdoor;
    // public GameObject enemyGroup;
    // public GameObject[] switch1On;
    // public GameObject[] switch1Off;
    // public GameObject[] switch2On;
    // public GameObject[] switch2Off;
    // public GameObject BossTV;

    // /// <summary>
    // /// CELL PHONE EVENT OBJECTS
    // /// </summary>
    // public GameObject cellPhone;
    // public SpriteRenderer cellPhoneSR;
    // public Sprite cellPhoneOn;
    // public TeleporterScript cellPhoneDoor;
    // public GameObject cellPhoneFightTrigger;

    // public bool sideDoorLock = false;
    // public bool secondEnemyActive = true;
    // public bool secondBlockerActive;
    // public bool swordBlockActive;
    // public bool groupEnemyActive = true;
    // public bool thirdEnemyActive = true;
    // public bool bossEnemyActive = true;
    // public bool switchOne;
    // public bool switchTwo;
    

    // [System.Serializable]
    // public struct LOneData
    // {
    //     public bool sideDoor;
    //     public bool secondEnemy;
    //     public bool secondBlocker;
    //     public bool swordBlock;
    //     public bool groupEnemy;
    //     public bool thirdEnemy;
    //     public bool bossEnemy;
    //     public bool switchOneOn;
    //     public bool switchTwoOn;
    // }

    void Start()
    {

        handler = GameObject.Find("/UI/VignetteController").GetComponent<CutsceneScript>();
    	movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        // mps.PlaySong("Sounds/Music/Floor_Mood",1,true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EntryEvent() {
        string[] intro =
        {
            "EXTREMELY LOUD VACUUM NOISES ECHO AROUND YOU!!!!!!",
            "\"Maybe coming here wasn't such a good idea.\""
        };
        handler.StartScene(intro);
    }

    public void commonMegaFightInit (){
    	GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
        btl.transform.position = new Vector3(1000, 1000, 0);
        CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        cs.sub = CameraSubject.battle;
        cs.Battle = btl;
        movscript.battle = btl;
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
		bm.InitializeBattle(EnemyFactory.EnemyType.megavroomer);
        bm.songloc = "Sounds/Music/Fight_Mood";
        movscript.LockMovement();
        // mps.PlaySong("Sounds/Music/Fight_Mood", 2, true);
    }

    public void StartMega0() {
        string[] dialogue =
        {
            "Time to suck it up.",
            "Vroomer Suck!"
        };
        string name = "Mega Vroomer Thug";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(InitM0Fight);
    }

    public void InitM0Fight() {
        commonMegaFightInit();
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.SetBattleEndCallback(EndM0Fight);
        Destroy(M0);
        M0Active = false;
        M0.SetActive(M0Active);
    }

    public void EndM0Fight() {
        movscript.UnlockMovement();
        string[] dialogue =
        {
            "Hehe, theres a lot more of us."
        };
        string name = "Mega Vroomer Thug";
        handler.StartDialogue(dialogue, name);
	}

	public void StartMega1() {
        string[] dialogue =
        {
            "Time to suck it up.",
            "Vroomer Suck!"
        };
        string name = "Mega Vroomer Thug";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(InitM1Fight);
    }

    public void InitM1Fight() {
        commonMegaFightInit();
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.SetBattleEndCallback(EndM1Fight);
        Destroy(M1);
        M0Active = false;
        M1.SetActive(M1Active);
    }

    public void EndM1Fight() {
        movscript.UnlockMovement();
        string[] dialogue =
        {
            "Hehe, theres a lot more of us."
        };
        string name = "Mega Vroomer Thug";
        handler.StartDialogue(dialogue, name);
	} 

	public void StartMega2() {
        string[] dialogue =
        {
            "Time to suck it up.",
            "Vroomer Suck!"
        };
        string name = "Mega Vroomer Thug";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(InitM2Fight);
    }

    public void InitM2Fight() {
        commonMegaFightInit();
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.SetBattleEndCallback(EndM2Fight);
        Destroy(M2);
        M2Active = false;
        M2.SetActive(M2Active);
    }

    public void EndM2Fight() {
        movscript.UnlockMovement();
        string[] dialogue =
        {
            "Hehe, theres a lot more of us."
        };
        string name = "Mega Vroomer Thug";
        handler.StartDialogue(dialogue, name);
	}

	public void StartMega3() {
        string[] dialogue =
        {
            "Time to suck it up.",
            "Vroomer Suck!"
        };
        string name = "Mega Vroomer Thug";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(InitM3Fight);
    }

    public void InitM3Fight() {
        commonMegaFightInit();
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.SetBattleEndCallback(EndM3Fight);
        Destroy(M3);
        M3Active = false;
        M3.SetActive(M3Active);
    }

    public void EndM3Fight() {
        movscript.UnlockMovement();
        string[] dialogue =
        {
            "Hehe, theres a lot more of us."
        };
        string name = "Mega Vroomer Thug";
        handler.StartDialogue(dialogue, name);
	}             

	public void StartMega4() {
        string[] dialogue =
        {
            "Time to suck it up.",
            "Vroomer Suck!"
        };
        string name = "Mega Vroomer Thug";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(InitM4Fight);
    }

    public void InitM4Fight() {
        commonMegaFightInit();
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.SetBattleEndCallback(EndM4Fight);
        Destroy(M4);
        M4Active = false;
        M4.SetActive(M4Active);
    }

    public void EndM4Fight() {
        movscript.UnlockMovement();
        string[] dialogue =
        {
            "Hehe, theres a lot more of us."
        };
        string name = "Mega Vroomer Thug";
        handler.StartDialogue(dialogue, name);
	}    

	public void StartMega5() {
        string[] dialogue =
        {
            "Time to suck it up.",
            "Vroomer Suck!"
        };
        string name = "Mega Vroomer Thug";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(InitM5Fight);
    }

    public void InitM5Fight() {
        commonMegaFightInit();
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.SetBattleEndCallback(EndM5Fight);
        Destroy(M5);
        M5Active = false;
        M5.SetActive(M5Active);
    }

    public void EndM5Fight() {
        movscript.UnlockMovement();
        string[] dialogue =
        {
            "Hehe, theres a lot more of us."
        };
        string name = "Mega Vroomer Thug";
        handler.StartDialogue(dialogue, name);
	}    











    // public void DisappearSwordObstacles()
    // {
    //     swordBlockActive = false;
    //     swordObstacles.SetActive(swordBlockActive);
    // }
    // public void ReappearSwordObstacles()
    // {
    //     swordBlockActive = true;
    //     swordObstacles.SetActive(swordBlockActive);
    // }
    // public void DisappearSecondRoomBlocker()
    // {
    //     secondBlockerActive = false;
    //     secondRoomBlocker.SetActive(secondBlockerActive);
    // }
    // public void ReappearSecondRoomBlocker()
    // {
    //     secondBlockerActive = true;
    //     secondRoomBlocker.SetActive(secondBlockerActive);
    // }
    // public void SideroomDoorLock()
    // {
    //     sideDoorLock = true;
    //     sideroomdoor.locked = sideDoorLock;
    //     string[] eventtext = { "You hear the door click behind you." };
    //     handler.StartScene(eventtext);
    // }
    // public void SideroomDoorUnlock()
    // {
    //     sideDoorLock = false;
    //     sideroomdoor.locked = sideDoorLock;
    //     string[] eventtext = { "You hear the door unlock behind you." };
    //     handler.StartScene(eventtext);
    // }
    // public void StartSecondRoomEnemyEvent()
    // {
    //     string[] dialogue =
    //     {
    //         "Hey, kid.",
    //         "Imma hack you up."
    //     };
    //     string name = "Maddened Lappy?";
    //     handler.StartDialogue(dialogue, name);
    //     handler.SetCallback(InitializeSecondRoomEnemyFight);
    // }
    // public void EndSecondRoomEnemyEvent()
    // {
    //     // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    //     movscript.UnlockMovement();
    //     string[] dialogue =
    //     {
    //         "Hehe, theres a lot of items for you to take...",
    //         "Just behind the door."
    //     };
    //     string name = "Broken Lappy";
    //     handler.StartDialogue(dialogue, name);
    //     // SideroomDoorUnlock();
    // }
    // public void InitializeSecondRoomEnemyFight()
    // {
    //     // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    //     GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
    //     btl.transform.position = new Vector3(1000, 1000, 0);
    //     CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
    //     cs.sub = CameraSubject.battle;
    //     cs.Battle = btl;
    //     movscript.battle = btl;
    //     BattleMasterScript bm;
    //     bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
    //     bm.InitializeBattle(EnemyFactory.EnemyType.lappy);
    //     bm.SetBattleEndCallback(EndSecondRoomEnemyEvent);
    //     //Destroy(SecondRoomEnemy);
    //     secondEnemyActive = false;
    //     SecondRoomEnemy.SetActive(secondEnemyActive);
    //     movscript.LockMovement();
    //     bm.songloc = "Sounds/Music/Fight_Mood";
    //     mps.PlaySong("Sounds/Music/Fight_Mood", 2, true);
    // }
    // public void InitializeGroupEnemyFight()
    // {
    //     // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    //     GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
    //     btl.transform.position = new Vector3(1000, 1000, 0);
    //     CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
    //     cs.sub = CameraSubject.battle;
    //     cs.Battle = btl;
    //     movscript.battle = btl;
    //     BattleMasterScript bm;
    //     List<EnemyFactory.EnemyType> enemies = new List<EnemyFactory.EnemyType>
    //     {
    //         EnemyFactory.EnemyType.crashedregister,
    //         EnemyFactory.EnemyType.barcodeimprinter,
    //         EnemyFactory.EnemyType.enlightenedmonitor
    //     };
    //     bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
    //     bm.InitializeBattle(enemies);
    //     //Destroy(enemyGroup);
    //     groupEnemyActive = false;
    //     enemyGroup.SetActive(groupEnemyActive);
    //     bm.songloc = "Sounds/Music/Fight_Mood";
    //     mps.PlaySong("Sounds/Music/Fight_Mood", 2, true);
    // }
    // public void StartEnemyGroupDrop()
    // {
    //     movscript.LockMovement();
    //     enemyGroup.transform.localPosition = new Vector3(0, 15, 0);
    //     StartCoroutine(DropEnemyGroup());
    // }
    // public void OpenedTrapBox()
    // {
    //     string[] dialogue =
    //     {
    //         "You opened the box.",
    //         "There seems to only be a receipt in the box, printed with the following phrase...",
    //         "\"S U C K E R .\""
    //     };
    //     handler.StartScene(dialogue);
    //     handler.SetCallback(StartEnemyGroupDrop);
    // }

    // private IEnumerator DropEnemyGroup()
    // {
    //     while (enemyGroup.transform.localPosition.y > 0)
    //     {
    //         enemyGroup.transform.localPosition -= new Vector3(0, 10.0f / 60.0f, 0);
    //         yield return null;
    //     }
    //     yield return new WaitForSeconds(1.0f);
    //     StartGroupEnemyFight();

    // }

    // private void StartGroupEnemyFight()
    // {
    //     movscript.LockMovement();
    //     CameraShader cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
    //     cs.StartWipe(Resources.Load<Texture>("Textures/weirdspiralwipe"), Resources.Load<Texture>("Textures/screenwipeouttex"), InitializeGroupEnemyFight, null, 1.0f, 3.0f);
    //     mps.PlaySong("Sounds/Music/O_SHIT_I_ENCOUNTERED_AN_ENEMY_BUT_ITS_SHORTER", 3, false);
    // }
    // public void StartSecondRoomEnemyFight()
    // {
    //     movscript.LockMovement();
    //     CameraShader cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
    //     cs.StartWipe(Resources.Load<Texture>("Textures/weirdspiralwipe"), Resources.Load<Texture>("Textures/screenwipeouttex"), InitializeSecondRoomEnemyFight, null, 1.0f, 3.0f);
    //     mps.PlaySong("Sounds/Music/O_SHIT_I_ENCOUNTERED_AN_ENEMY_BUT_ITS_SHORTER", 3, false);
    // }

    //  public void StartThirdRoomEnemyEvent()
    // {
    //     string[] dialogue =
    //     {
    //         "I swear you leave your stuff alone for a second..",
    //         "...and someone takes all of your sticks and snacks...",
    //         "...GIVE THEM BACK!!!"
    //     };
    //     string name = "Possesive Vroomer";
    //     handler.StartDialogue(dialogue, name);
    //     handler.SetCallback(StartThirdRoomEnemyFight);
    // }
    // public void RemoveTV()
    // {
    //     //Destroy(BossTV);
    //     bossEnemyActive = false;
    //     BossTV.SetActive(bossEnemyActive);
    // }
    // public void EndTVBossEvent()
    // {
    //     movscript.UnlockMovement();
    //     string[] dialogue =
    //     {
    //         "If only my demise were televised...",
    //         "...If you still want a chance to make it as a main character;",
    //         "Find the Vroomer."
    //     };
    //     string name = "Erudite TV";
    //     handler.StartDialogue(dialogue, name);
    //     handler.SetCallback(RemoveTV);
    // }
    // public void InitializeTVBossFight()
    // {
    //     // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    //     GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
    //     btl.transform.position = new Vector3(1000, 1000, 0);
    //     CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
    //     cs.sub = CameraSubject.battle;
    //     cs.Battle = btl;
    //     movscript.battle = btl;
    //     BattleMasterScript bm;
    //     bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
    //     bm.InitializeBattle(EnemyFactory.EnemyType.tvboss);
    //     bm.SetBattleEndCallback(EndTVBossEvent);
    //     movscript.LockMovement();
    //     mps.PlaySong("Sounds/Music/Boss_Mood", 2, true);
    //     bm.songloc = "Sounds/Music/Boss_Mood";
    // }
    // public void StartTVBossFight()
    // {
    //     movscript.LockMovement();
    //     CameraShader cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
    //     cs.StartWipe(Resources.Load<Texture>("Textures/weirdspiralwipe"), Resources.Load<Texture>("Textures/screenwipeouttex"), InitializeTVBossFight, null, .75f, 3.0f);
    //     mps.PlaySong("Sounds/Music/O_SHIT_I_ENCOUNTERED_AN_ENEMY", 3, false);
    // }
    // public void StartTVBossEvent()
    // {
    //     string[] dialogue =
    //     {
    //         "I was warned you were coming.",
    //         "Unfortunately for you, the cue card has been clapped, the action has started, the cameras already rolling...",
    //         "...and you are off-screen."
    //     };
    //     string name = "Smarter TV?";
    //     handler.StartDialogue(dialogue, name);
    //     handler.SetCallback(StartTVBossFight);
    // }
    // public void EndThirdRoomEnemyEvent()
    // {
    //     // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    //     movscript.UnlockMovement();
    //     string[] dialogue =
    //     {
    //         "My boss will hear about this!"
    //     };
    //     string name = "Possesive Vroomer";
    //     handler.StartDialogue(dialogue, name);
    //     // SideroomDoorUnlock();
    // }
    // public void InitializeThirdRoomEnemyFight()
    // {
    //     // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    //     GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
    //     btl.transform.position = new Vector3(1000, 1000, 0);
    //     CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
    //     cs.sub = CameraSubject.battle;
    //     cs.Battle = btl;
    //     movscript.battle = btl;
    //     BattleMasterScript bm;
    //     bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
    //     bm.InitializeBattle(EnemyFactory.EnemyType.vroomer);
    //     bm.SetBattleEndCallback(EndThirdRoomEnemyEvent);
    //     //Destroy(ThirdRoomEnemy);
    //     thirdEnemyActive = false;
    //     ThirdRoomEnemy.SetActive(thirdEnemyActive);
    //     movscript.LockMovement();
    //     bm.songloc = "Sounds/Music/Fight_Mood";
    //     mps.PlaySong("Sounds/Music/Fight_Mood", 2, true);
    // }
    // public void StartThirdRoomEnemyFight()
    // {
    //     movscript.LockMovement();
    //     CameraShader cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
    //     cs.StartWipe(Resources.Load<Texture>("Textures/weirdspiralwipe"), Resources.Load<Texture>("Textures/screenwipeouttex"), InitializeThirdRoomEnemyFight, null, 1.0f, 3.0f);
    //     mps.PlaySong("Sounds/Music/O_SHIT_I_ENCOUNTERED_AN_ENEMY_BUT_ITS_SHORTER", 3, false);
    // }

    // public void TriggerPostThirdBlockerEvent()
    // {
    // 	string[] eventtext = { "Can't go there. Theres too many vroomers there to fight!"};
    //     handler.StartScene(eventtext);
    //     movscript.gameObject.GetComponent<Transform>().position = movscript.gameObject.GetComponent<Transform>().position + new Vector3( (float) .25,0,0);
    // }

    // public void SwitchOneOn()
    // {
    //     switchOne = true;
    //     for (int i = 0; i < switch1On.Length; i++)
    //     {
    //         switch1On[i].SetActive(switchOne);
    //     }
    //     for (int i = 0; i < switch1Off.Length; i++)
    //     {
    //         switch1Off[i].SetActive(!switchOne);
    //     }
    // }
    // public void SwitchOneOff()
    // {
    //     switchOne = false;
    //     for (int i = 0; i < switch1On.Length; i++)
    //     {
    //         switch1On[i].SetActive(switchOne);
    //     }
    //     for (int i = 0; i < switch1Off.Length; i++)
    //     {
    //         switch1Off[i].SetActive(!switchOne);
    //     }
    // }
    // public void SwitchTwoOn()
    // {
    //     switchTwo = true;
    //     for (int i = 0; i < switch2On.Length; i++)
    //     {
    //         switch2On[i].SetActive(switchTwo);
    //     }
    //     for (int i = 0; i < switch2Off.Length; i++)
    //     {
    //         switch2Off[i].SetActive(!switchTwo);
    //     }
    // }
    // public void SwitchTwoOff()
    // {
    //     switchTwo = false;
    //     for (int i = 0; i < switch2On.Length; i++)
    //     {
    //         switch2On[i].SetActive(switchTwo);
    //     }
    //     for (int i = 0; i < switch2Off.Length; i++)
    //     {
    //         switch2Off[i].SetActive(!switchTwo);
    //     }
    // }

    // public void CellPhoneTrigger()
    // {

    //     string[] dialogue =
    //     {
    //         "I'm not broken yet ya idiot!",
    //         "Get over here so I can kick your butt!"
    //     };
    //     string name = "Cell Phone";
    //     cellPhoneSR.sprite = cellPhoneOn;
    //     cellPhoneFightTrigger.SetActive(true);
    //     handler.StartDialogue(dialogue, name);
    // }

    // public void StartCellPhoneFight()
    // {
    //     movscript.LockMovement();
    //     CameraShader cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
    //     cs.StartWipe(Resources.Load<Texture>("Textures/weirdspiralwipe"), Resources.Load<Texture>("Textures/screenwipeouttex"), InitializeCellPhoneFight, null, 1.0f, 3.0f);
    //     mps.PlaySong("Sounds/Music/O_SHIT_I_ENCOUNTERED_AN_ENEMY_BUT_ITS_SHORTER", 3, false);
    // }
    // public void InitializeCellPhoneFight()
    // {

    //     // PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    //     GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
    //     btl.transform.position = new Vector3(1000, 1000, 0);
    //     CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
    //     cs.sub = CameraSubject.battle;
    //     cs.Battle = btl;
    //     movscript.battle = btl;
    //     BattleMasterScript bm;
    //     bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
    //     bm.InitializeBattle(EnemyFactory.EnemyType.selfphone);
    //     bm.SetBattleEndCallback(EndCellPhoneEvent);
    //     bm.songloc = "Sounds/Music/Fight_Mood";
    //     movscript.LockMovement();
    //     bm.songloc = "Sounds/Music/Fight_Mood";
    //     mps.PlaySong("Sounds/Music/Fight_Mood", 2, true);
    // }
    // public void RemoveCellPhone()
    // {
    //     cellPhone.SetActive(false);
    // }
    // public void EndCellPhoneEvent()
    // {
    //     movscript.UnlockMovement();
    //     string[] dialogue =
    //    {
    //         "Heh... you got me good back there pal, even though I tried to do you dirty.",
    //         "Tell you what - I remotely unlocked the backroom's door for you, there's something good in there. You deserve it.",
    //         "Now...SeeRee...set a reminder for 6:00pm today...",
    //         "For my appointment in Heck..."
    //     };
    //     string name = "Cell Phone";
    //     cellPhoneDoor.locked = false;
    //     handler.StartDialogue(dialogue, name);
    //     handler.SetCallback(RemoveCellPhone);
    // }
    // public void CellPhoneFightEvent()
    // {
    //     string[] dialogue =
    //     {
    //         "You just fell for the oldest trick in the phone book, pal!"
    //     };
    //     string name = "Cell Phone";
    //     handler.StartDialogue(dialogue, name);
    //     handler.SetCallback(StartCellPhoneFight);
    // }

    [SerializeField]
    // private LOneData LevelOneData;

    public string OnSave()
    {
    	return "";
        // return JsonUtility.ToJson(new LOneData() { sideDoor = sideDoorLock,
        //                                            secondEnemy = secondEnemyActive,
        //                                            secondBlocker = secondBlockerActive,
        //                                            swordBlock = swordBlockActive,
        //                                            groupEnemy = groupEnemyActive,
        //                                            thirdEnemy = thirdEnemyActive,
        //                                            bossEnemy = bossEnemyActive,
        //                                            switchOneOn = switchOne,
        //                                            switchTwoOn = switchTwo
        //                                            });
    }

    public void OnLoad(string data)
    {
        // LevelOneData = JsonUtility.FromJson<LOneData>(data);

        // //retrieve information
        // sideDoorLock = LevelOneData.sideDoor;
        // secondEnemyActive = LevelOneData.secondEnemy;
        // secondBlockerActive = LevelOneData.secondBlocker;
        // swordBlockActive = LevelOneData.swordBlock;
        // groupEnemyActive = LevelOneData.groupEnemy;
        // thirdEnemyActive = LevelOneData.thirdEnemy;
        // bossEnemyActive = LevelOneData.bossEnemy;
        // switchOne = LevelOneData.switchOneOn;
        // switchTwo = LevelOneData.switchTwoOn;

        // //set state
        // sideroomdoor.locked = sideDoorLock;
        // SecondRoomEnemy.SetActive(secondEnemyActive);
        // secondRoomBlocker.SetActive(secondBlockerActive);
        // swordObstacles.SetActive(swordBlockActive);
        // enemyGroup.SetActive(groupEnemyActive);
        // ThirdRoomEnemy.SetActive(thirdEnemyActive);
        // BossTV.SetActive(bossEnemyActive);

        // if (switchOne)
        //     SwitchOneOn();
        // else
        //     SwitchOneOff();

        // if (switchTwo)
        //     SwitchTwoOn();
        // else
        //     SwitchTwoOff();
    }

    public bool OnSaveCondition()
    {
        return true;
    }
}
