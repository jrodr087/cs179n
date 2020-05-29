using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineDialogue : MonoBehaviour
{
	public string[] Dialogue;
	private CutsceneScript handler;
    private PlayableDirector gameTimeline;
    //public PlayerMovement movscript;
    // Start is called before the first frame update
    void Start()
    {
        handler = GameObject.Find("/UI/VignetteController").GetComponent<CutsceneScript>();
        gameTimeline = GameObject.Find("TimelineManager").GetComponent<PlayableDirector>();
        //movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        handler.StartSceneForTimeline(Dialogue);
        if (handler.getCurrState() == CutsceneScript.states.vigentering){
                gameTimeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
        else if (handler.getCurrState() == CutsceneScript.states.vigexiting){
            gameTimeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
    }
}
