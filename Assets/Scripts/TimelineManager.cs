using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineManager : MonoBehaviour
{
	public PlayerMovement movscript;
	public PlayableDirector director;
    // Start is called before the first frame update
    void Start()
    {
        movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (director.state != PlayState.Playing){
        	movscript.UnlockMovement();
        }
        else{
        	movscript.LockMovement();
        }
    }

    public void TriggerEnterTutorialZone()
    {
        SceneManager.LoadScene("Level1");
    }
}
