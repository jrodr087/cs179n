using Lowscope.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Level2EventHandler : MonoBehaviour, ISaveable
{

    public MusicPlayerScript mps;
    
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

    public GameObject OMNI;
    public bool OMNIActive = true;

    private BattleMasterScript bm;
    private bool entered = false;

    [System.Serializable]
    public struct LTwoData
    {
        public bool bM0;
        public bool bM1;
        public bool bM2;
        public bool bM3;
        public bool bM4;
        public bool bM5;
        public bool bOm;
    }

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
    	if (!entered){
    		string[] intro =
	        {
	            "EXTREMELY LOUD VACUUM NOISES ECHO AROUND YOU!!!!!!",
	            "\"Maybe coming here wasn't such a good idea.\""
	        };
	        handler.StartScene(intro);
	        entered = true;
	        // mps.PlaySong("Sounds/Music/Floor_Mood",1,true);
    	}
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
        mps.PlaySong("Sounds/Music/Fight_Mood", 1, true);
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
            "You came to the wrong place.",
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
            "I may not be the strongest vroomer, but I'm stronger than you.",
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
            "Ooh I haven't fought in a while.",
            "Time to get back into it.",
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
            "My boss is even bigger!!!",
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
            "Haha, theres still people in this city!.",
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


    public void StartOMNI() {
        string[] dialogue =
        {
            "I AM the OMNIVROOMER",
            "I have rebuilt myself piece by piece to who I am",
            "You are unworthy",
        };
        string name = "OmniVroomer";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(InitOMNIFight);
    }

    public void InitOMNIFight() {
        commonMegaFightInit();
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.SetBattleEndCallback(EndOMNIFight);
        Destroy(OMNI);
        OMNIActive = false;
        OMNI.SetActive(OMNIActive);
    }

    public void EndOMNIFight() {
        movscript.UnlockMovement();
        string[] dialogue =
        {
            "NOOOOO, I was going to be the greatest Vroomer Ever!!!!"
        };
        string name = "OmniVroomer";
        handler.StartDialogue(dialogue, name);
    }  

    [SerializeField]
    private LTwoData LevelTwoData;

    public string OnSave()
    {
        return JsonUtility.ToJson(new LTwoData() { 
        	bM0 = M0Active,
        	bM1 = M1Active,
        	bM2 = M2Active,
        	bM3 = M3Active,
        	bM4 = M4Active,
        	bM5 = M5Active,
            bOm = OMNIActive,
        });
    }

    public void OnLoad(string data)
    {
        LevelTwoData = JsonUtility.FromJson<LTwoData>(data);

        //retrieve information
        M0Active = LevelTwoData.bM0;
    	M1Active = LevelTwoData.bM1;
    	M2Active = LevelTwoData.bM2;
    	M3Active = LevelTwoData.bM3;
    	M4Active = LevelTwoData.bM4;
    	M5Active = LevelTwoData.bM5;

        OMNIActive = LevelTwoData.bOm;

        // sideDoorLock = LevelOneData.sideDoor;
        // secondEnemyActive = LevelOneData.secondEnemy;
        // secondBlockerActive = LevelOneData.secondBlocker;
        // swordBlockActive = LevelOneData.swordBlock;
        // groupEnemyActive = LevelOneData.groupEnemy;
        // thirdEnemyActive = LevelOneData.thirdEnemy;
        // bossEnemyActive = LevelOneData.bossEnemy;
        // switchOne = LevelOneData.switchOneOn;
        // switchTwo = LevelOneData.switchTwoOn;

        //set state
    	M0.SetActive(M0Active);
    	M1.SetActive(M1Active);
    	M2.SetActive(M2Active);
    	M3.SetActive(M3Active);
    	M4.SetActive(M4Active);
    	M5.SetActive(M5Active);

        OMNI.SetActive(OMNIActive);
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
