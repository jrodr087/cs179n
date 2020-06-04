using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

public class Level0EventHandler : MonoBehaviour
{
	private CutsceneScript handler;
    private PlayerMovement movscript;
    private PlayableDirector director;
    public GameObject doctorTriggers;
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
    public void TriggerEnterLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void TriggerDoctor1()
    {
        string[] dialogue =
        {
            "Oh my Gosh!!",
            "You're Alive???",
            "Errr.... I mean.... Great to see you",
            "It looks like you've finally awoken from your weeks long slumber!",
            "A rogue Vroomer knocked you out in the uprising",
            "....",
            "Oh... you don't remember the uprising?"
        };
        string name = "A Startled Doctor";
        director = GameObject.Find("Timelines/DoctorUpTrigger").GetComponent<PlayableDirector>();

        if (director != null){
            director.Play();
            while (director.state = PlayState.Playing){
                movscript.LockMovement();
            }
            movscript.UnlockMovement();
        }

        handler.StartDialogue(dialogue, name);
        doctorTriggers.SetActive(false);
    }
}
