using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1EventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject swordObstacles;
    public GameObject SelfPhone;
    public TeleporterScript sideroomdoor;
    private CutsceneScript handler;
    void Start()
    {

        handler = GameObject.Find("/UI/VignetteController").GetComponent<CutsceneScript>();
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
    public void StartSelfPhoneEvent()
    {
        string[] dialogue =
        {
            "Hey, kid.",
            "Better call an ambulance."
        };
        string name = "Cell Phone?";
        handler.StartDialogue(dialogue, name);
        handler.SetCallback(StartSelfPhoneFight);
    }
    public void EndSelfPhoneEvent()
    {
        PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        movscript.UnlockMovement();
        SideroomDoorUnlock();
    }
    public void InitializeSelfPhoneFight()
    {
        PlayerMovement movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
        btl.transform.position = new Vector3(1000, 1000, 0);
        CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        cs.sub = CameraSubject.battle;
        cs.Battle = btl;
        movscript.battle = btl;
        BattleMasterScript bm;
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.InitializeBattle(EnemyFactory.EnemyType.selfphone);
        bm.SetBattleEndCallback(EndSelfPhoneEvent);
        Destroy(SelfPhone);
        movscript.LockMovement();
    }
    public void StartSelfPhoneFight()
    {
        CameraShader cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
        cs.StartWipe(Resources.Load<Texture>("Textures/weirdspiralwipe"), Resources.Load<Texture>("Textures/screenwipeouttex"), InitializeSelfPhoneFight, null, 1.0f, 3.0f);
    }

}
