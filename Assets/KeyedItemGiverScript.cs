using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyedItemGiverScript : MonoBehaviour
{
    public string[] interactableDialogue;
    private SpriteRenderer sr;
    private CutsceneScript handler;
    public Sprite openedSprite;
    private PlayerData pd;
    private PlayerMovement pm;
    public bool opened = false;
    public string[] keyedText;
    public string[] openedText;
    private bool inside = false;
    public ItemDirectory.ItemIndex itemId;
    public ItemDirectory.ItemIndex keyId;

    public ChoiceMenuScript choiceMenuScript;
    public ItemMenuScript itemMenuScript;

    private ItemDirectory id = new ItemDirectory();
    AudioSource itemReceivedStinger;

    // Start is called before the first frame update
    void Start()
    {
        handler = GameObject.Find("/UI/VignetteController").GetComponent<CutsceneScript>();
        choiceMenuScript = GameObject.Find("/UI/ChoiceMenu").GetComponent<ChoiceMenuScript>();
        itemMenuScript = GameObject.Find("/UI/MenuPanel/ItemMenu").GetComponent<ItemMenuScript>();
        choiceMenuScript.closeMenu();
        GameObject player = GameObject.Find("Player");
        pd = player.GetComponent<PlayerData>();
        pm = player.GetComponent<PlayerMovement>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        itemReceivedStinger = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && inside && !pm.GetMovementLock())
        {
            if (!opened)
            {
                handler.StartScene(interactableDialogue);
                if (pd.items.Contains(keyId)){
                    choiceMenuScript.openMenu(keyId, id.dir[ (int) keyId].name, unlockBox);
                }
            }
            else
            {
                handler.StartScene(openedText);
            }
        }
    }

    public void unlockBox(){
        handler.StartSceneFromLock(keyedText);
        opened = true;
        itemReceivedStinger.PlayOneShot((AudioClip)Resources.Load("Sounds/ItemFound"));
        sr.sprite = openedSprite;
        choiceMenuScript.closeMenu();
        pd.RemoveItem(keyId);
        pd.GiveItem(itemId);
        itemMenuScript.UpdateButtons();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            inside = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            inside = false;
        }
    }

}
