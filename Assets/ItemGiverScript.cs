using Lowscope.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemGiverScript : MonoBehaviour, ISaveable
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

    [System.Serializable]
    public struct IData
    {
        public string[] IinteractableDialogue;
        public SpriteRenderer Isr;
        public CutsceneScript Ihandler;
        public Sprite IopenedSprite;
        public PlayerData Ipd;
        public PlayerMovement Ipm;
        public bool Iopen;
        public string[] IopenedText;
        public bool isInside;
    }

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

    [SerializeField]
    private IData itemData;
    public string OnSave()
    {
        return JsonUtility.ToJson(new IData() { IinteractableDialogue = interactableDialogue,
                                                Isr = sr,
                                                Ihandler = handler,
                                                IopenedSprite = openedSprite,
                                                Ipd = pd,
                                                Ipm = pm,
                                                Iopen = opened,
                                                IopenedText = openedText,
                                                isInside = inside });
    }

    public void OnLoad(string data)
    {
        itemData = JsonUtility.FromJson<IData>(data);
        
        interactableDialogue = itemData.IinteractableDialogue;
        sr = itemData.Isr;
        handler = itemData.Ihandler;
        openedSprite = itemData.IopenedSprite;
        pd = itemData.Ipd;
        pm = itemData.Ipm;
        opened = itemData.Iopen;
        openedText = itemData.IopenedText;
        inside = itemData.isInside;
        if(opened)
        {
            this.GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }

    public bool OnSaveCondition()
    {
        return true;
    }
}
