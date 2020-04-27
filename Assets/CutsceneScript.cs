﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneScript : MonoBehaviour
{
    public Image vigtop;
    public Image vigbot;
    public PlayerMovement movscript;
    private int vignettespeed = 10;
    private int vigspot = 64;
    private RectTransform vigtoptransform;
    private RectTransform vigbottransform;
    public Text txt;
    private int textspeed = 4;
    private string currstring;
    private string fullstring;
    private string[] scenestrings;
    private int stringindex;
    private int charindex;
    private int timer;
    private enum states { vigout,vigentering,vigin,vigexiting}
    private states currstate = states.vigout;
    AudioSource txtsfx;

    // Start is called before the first frame update
    void Start()
    {
        movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        vigtoptransform = vigtop.GetComponent<RectTransform>();
        vigbottransform = vigbot.GetComponent<RectTransform>();
        txtsfx = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currstate == states.vigentering)
        {
            vigtoptransform.anchoredPosition = new Vector2(vigtoptransform.anchoredPosition.x, vigtoptransform.anchoredPosition.y - vignettespeed);
            vigbottransform.anchoredPosition = new Vector2(vigbottransform.anchoredPosition.x, vigbottransform.anchoredPosition.y + vignettespeed);
            if (vigbottransform.anchoredPosition.y >= 0)
            {
                currstate = states.vigin;
                vigbottransform.anchoredPosition = new Vector2(vigbottransform.anchoredPosition.x, 0);
                vigtoptransform.anchoredPosition = new Vector2(vigbottransform.anchoredPosition.x, 0);
            }

        }
        else if (currstate == states.vigin)
        {
            if (charindex < fullstring.Length)
            {
                if (timer > 0)
                {
                    timer--;
                }
                else
                {
                    charindex++;
                    currstring = fullstring.Substring(0, charindex);
                    if (Input.GetKey("space"))
                    {
                        timer = 0;
                    }
                    else
                    {
                        if (currstring[currstring.Length-1] == '.') 
                        {
                            timer = 45;
                        }
                        else if (fullstring[currstring.Length -1] == ',' || currstring[currstring.Length - 1] == '-')
                        {
                            timer = 30;
                        }
                        else 
                        {
                            timer = textspeed;
                        }
                        
                    }
                    if (!txtsfx.isPlaying)
                    {
                        float pitchrng = Random.Range(0.0f, 0.1f);
                        txtsfx.pitch = 1.0f + pitchrng;
                        txtsfx.Play();
                    }
                    txt.text = currstring;
                }
            }
            else
            {
                if (Input.GetKeyDown("space"))
                {
                    stringindex++;
                    if (stringindex < scenestrings.Length)
                    {
                        charindex = 0;
                        currstring = "";
                        fullstring = scenestrings[stringindex];
                    }
                    else
                    {
                        currstate = states.vigexiting;
                    }
                }
            }
        }
        else if (currstate == states.vigexiting)
        {
           vigtoptransform.anchoredPosition = new Vector2(vigtoptransform.anchoredPosition.x, vigtoptransform.anchoredPosition.y + vignettespeed);
            vigbottransform.anchoredPosition = new Vector2(vigbottransform.anchoredPosition.x, vigbottransform.anchoredPosition.y - vignettespeed);
            if (vigbottransform.anchoredPosition.y <= -vigspot)
            {
                currstate = states.vigout;
                vigbottransform.anchoredPosition = new Vector2(vigbottransform.anchoredPosition.x, -vigspot);
                vigtoptransform.anchoredPosition = new Vector2(vigbottransform.anchoredPosition.x, vigspot);
                movscript.UnlockMovement();
            }

        }
        else if (currstate == states.vigout)
        {
            //if (Input.GetKeyDown("space"))
            //{
            //    string[] testarray = { "Testing the cutscene vignette.",
            //    "Testing a second line in the cutscene vignette",
            //    "Testing a longer line in the cutscene vignette. Testing a longer line in the cutscene vignette. Testing a longer line in the cutscene vignette. "};
            //    StartScene(testarray);
            //}
        }

    }
    public void StartScene(string[] strings)
    {
        if (currstate == states.vigout && !movscript.GetMovementLock())
        {
            movscript.LockMovement();
            scenestrings = strings;
            currstate = states.vigentering;
            currstring = "";
            fullstring = strings[0];
            stringindex = 0;
            charindex = 0;
            timer = textspeed;
            txt.text = "";
        }
    }
    
}
