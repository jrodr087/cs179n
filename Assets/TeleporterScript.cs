using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TeleporterScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject destination;
    public AudioClip sound;
    new AudioSource audio;
    private PlayerMovement movscript;
    private CameraShader cs;
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
    }
    void Teleport()
    {
        movscript.gameObject.GetComponent<Transform>().position = destination.GetComponent<BoxCollider2D>().transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && inside && !movscript.GetMovementLock())
        {
            movscript.LockMovement();
            cs.StartWipe(inwipe,outwipe,Teleport, movscript.UnlockMovement);
            if (sound != null)
            {
                audio.PlayOneShot(sound);
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
}
