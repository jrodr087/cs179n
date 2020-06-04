﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractableButton : MonoBehaviour
{

    public string[] switchOnText;
    public string[] switchOffText;
    public bool toggleable = false;
    public bool onDialogue = true;
    public bool offDialogue = true;
    public bool showTextEvenWhenNotToggleable = false;
    public bool switchedon = false;
    public UnityEvent onfunc;
    public UnityEvent offfunc;
    public AudioClip sound;
    public Sprite onsprite;
    public Sprite offsprite;
    new AudioSource audio;
    private CutsceneScript handler;
    private PlayerMovement movscript;
    private SpriteRenderer sr;
    private bool inside = false;
    // Start is called before the first frame update
    void Start()
    {
        handler = GameObject.Find("/UI/VignetteController").GetComponent<CutsceneScript>();
        audio = gameObject.AddComponent<AudioSource>();
        movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && inside && !movscript.GetMovementLock())
        {
            if (!switchedon)
            {
                if (onDialogue)
                {
                    handler.StartScene(switchOffText);
                }
                if (sound != null)
                {
                    audio.PlayOneShot(sound);
                }
                switchedon = true;
                onfunc.Invoke();
                sr.sprite = onsprite;
            }
            else
            {
                // Josiah - changed so toggleable bool actually keeps it from switching
                if (toggleable)
                {
                    if (offDialogue)
                    {
                        handler.StartScene(switchOffText);
                    }
                    switchedon = false;
                    if (sound != null)
                    {
                        audio.PlayOneShot(sound);
                    }
                    sr.sprite = offsprite;
                    offfunc.Invoke();
                }
                else if (showTextEvenWhenNotToggleable) //Jake - should have mentioned that it's intended behaviour to still trigger a scene using the switchOffText, but I created a separate bool for this case just to make it more explicit
                    //for when its intended
                {
                    if (offDialogue)
                    {
                        handler.StartScene(switchOffText);
                    }
                }
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