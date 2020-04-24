using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemGiverScript : MonoBehaviour
{
    public string[] interactableDialogue;
    private SpriteRenderer sr;
    private CutsceneScript handler;
    public Sprite openedSprite;
    private PlayerData pd;
    private PlayerMovement pm;
    public bool opened = false;
    public string[] openedText;
    private bool inside = false;
    public ItemDirectory.ItemIndex itemId;

    AudioSource itemReceivedStinger;
    // Start is called before the first frame update
    void Start()
    {
        handler = GameObject.Find("/UI/VignetteController").GetComponent<CutsceneScript>();
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
                opened = true;
                pd.GiveItem(itemId);
                itemReceivedStinger.PlayOneShot((AudioClip)Resources.Load("Sounds/ItemFound"));
                sr.sprite = openedSprite;
            }
            else
            {
                handler.StartScene(openedText);
            }
        }

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
