using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1EventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject swordObstacles;
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
    }
}
