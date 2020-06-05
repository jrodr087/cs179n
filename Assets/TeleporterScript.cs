using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TeleporterScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject destination;
    public AudioClip sound;
    public bool locked = false;
    public string[] locktext = { "It's locked." };
    public bool above;
    new AudioSource audio;
    private PlayerMovement movscript;
    private CameraShader cs;
    private CutsceneScript handler;
    private bool inside = false;
    private Texture inwipe;
    private Texture outwipe;
    void Start()
    {
        cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
        movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        inwipe = Resources.Load<Texture>("Textures/screenwipeintex");
        outwipe = Resources.Load<Texture>("Textures/screenwipeouttex");
        audio = gameObject.AddComponent<AudioSource>();
        handler = GameObject.Find("/UI/VignetteController").GetComponent<CutsceneScript>();
    }
    void BelowTeleport()
    {
        movscript.gameObject.GetComponent<Transform>().position = destination.GetComponent<BoxCollider2D>().transform.position;
    }
    void AboveTeleport()
    {
        movscript.gameObject.GetComponent<Transform>().position = destination.GetComponent<BoxCollider2D>().transform.position + new Vector3(0,5,0);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && inside && !movscript.GetMovementLock())
        {
            if (!locked)
            {
                movscript.LockMovement();
                if (above){
                    cs.StartWipe(inwipe, outwipe, AboveTeleport, movscript.UnlockMovement);
                }
                else {
                    cs.StartWipe(inwipe, outwipe, BelowTeleport, movscript.UnlockMovement);
                }
                if (sound != null)
                {
                    audio.PlayOneShot(sound);
                }
            }
            else
            {
                handler.StartScene(locktext);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            inside = true;
            Debug.Log("player entered teleporter zome");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            inside = false;
        }
    }

    public void Unlock(){
        locked = false;
    }

    public void Lock(){
        locked = true;
    }
}
