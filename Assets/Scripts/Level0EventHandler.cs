using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0EventHandler : MonoBehaviour
{
	private CutsceneScript handler;
    private PlayerMovement movscript;
    // Start is called before the first frame update
    void Start()
    {
        handler = GameObject.Find("/UI/VignetteController").GetComponent<CutsceneScript>();
    	movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
