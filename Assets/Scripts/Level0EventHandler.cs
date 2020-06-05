using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
//using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

public class Level0EventHandler : MonoBehaviour
{
	private CutsceneScript handler;
    private PlayerMovement movscript;
    private PlayableDirector director;
    private PlayerData pd;
    private string[] opening_dialogue =        
    {
            "Oh my Gosh!! ...You're Alive???",
            "Errr.... I mean.... Great to see you! You've finally awoken from your slumber.",
            "That rogue Vroomer must've knocked you out pretty darn hard during The Uprising! I didn't think you'd make it... It's been 3 long weeks after all.",
            "..Oh....you don't remember The Uprising? You lucky son of a gun.",
            "An Artificial Super Intelligence was created 3 weeks ago",
            "and within half a week, it propogated into every electronic -- Machines have gone haywire!!!",
            "And they want to use humans as biofuel! You ever seen the Matrix? We're about to look like that, buddy.",
            "Our town's already been commandeered by the AI's liutenant -- a omniscient smartcar. If only someone could kill the thing...",
            "Welp, now that you're well, you're going to have to leave. We need the bedspace for other patients.",
            "Here, you can take this water gun -- to defend yourself. It's not much, but it's something. Worst Sell probably has better."
    };

    // private string[] opening_dialogue =        
    // {
    //         "test."
    // };

    private string name = "A Startled Doctor";
    public GameObject doctorTriggers;
    public GameObject entranceTrigger;
    public GameObject hospitalDoor;
    public ItemDirectory.ItemIndex docsGift;
    public GameObject ItemTutorialTrigger;
    public GameObject StatTutorialTrigger;
    public GameObject LoneVroomerTrigger;
    public GameObject Level1Start;

    // Start is called before the first frame update
    void Start()
    {
        handler = GameObject.Find("/UI/VignetteController").GetComponent<CutsceneScript>();
    	movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        pd = GameObject.Find("Player").GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //When player enters room, he realizes he is in a hospital
    public void EnterRoom()
    {
        string[] dialogue = 
        {
            "Looks like you're in a hospital. Guess that's better than being dead???"
        };

        handler.StartScene(dialogue);
        entranceTrigger.SetActive(false);

    }

    public void TeleportToLevel1()
    {
        movscript.transform.position = Level1Start.transform.position;
    }

    //When player goes into door of Worst Buy, Level One begins
    public void TriggerEnterLevel1()
    {
        string[] dialogue = 
        {
            "Worst Sell? This is where the doctor said you could get weapons... "
        };

        handler.StartScene(dialogue);
        handler.SetCallback(TeleportToLevel1);
    }

    // When player gets near doctor, the doctor explains what happened to him


    public void TriggerDoctor()
    {
        TeleporterScript door;
        string[] dialogue = opening_dialogue;
        director = GameObject.Find("Timelines/Doctor2").GetComponent<PlayableDirector>();

        if (director != null){
            director.Play();
        }

        handler.StartDialogue(dialogue, name);
        doctorTriggers.SetActive(false);
        door = hospitalDoor.GetComponent<TeleporterScript>();
        door.Unlock();
        pd.GiveItem(docsGift);
        ItemTutorialTrigger.SetActive(true);
    }

    //After Doc gives item, you learn how to view and equip the item
    public void TriggerItemTutorial(){
        string[] dialogue = {
            "Press LEFT SHIFT to open the menu.",
            "Click ITEMS to see what Doc gave you and equip it."
        };
        handler.StartScene(dialogue);
        ItemTutorialTrigger.SetActive(false);
    }

    public void TriggerRoombahsMob(){
        string[] dialogue = {
            "Too many vroomers. No need for another coma, amateur.",
            "You need more weapons or more practice."
        };
        handler.StartScene(dialogue);
    }

    public void StatsTrigger(){
        string[] dialogue = {
            "What a mess. Looks like Worst Sell's been infiltrated by all its products.",
            "You better max out your stats before entering enemy territory",
            "Press LEFT SHIFT to open the menu. Click STATUS to allocate your stats points."
        };

        handler.StartScene(dialogue);
        StatTutorialTrigger.SetActive(false);

    }

    public void LoneVroomer(){
        string[] dialogue = {
            "A lone vroomer... It's kind of cute :(",
            "You might want to practice your combat skills on the lil' fella"
        };

        handler.StartScene(dialogue);
        LoneVroomerTrigger.SetActive(false);
    }

}
