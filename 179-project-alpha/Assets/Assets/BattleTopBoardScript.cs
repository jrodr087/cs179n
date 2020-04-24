using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTopBoardScript : MonoBehaviour
{
    public Text txt;
    public Image board;
    public int boardspeed = 10;
    private int boardheight = 128;
    private RectTransform boardt;
    private enum states {boardout,boardentering,boardin,boardexiting};
    private states currstate = states.boardout;
    private int timer = 0;
    public int textspeed = 3;
    private float boardtimer = 3.0f;
    private float currboardtimer = 0.0f;
    private string currstring = "";
    private string fullstring = "";
    private int charindex = 0;
    private string teststring = "Test,";
    AudioSource txtsfx;

    // Start is called before the first frame update
    void Start()
    {
        boardt = board.GetComponent<RectTransform>();
        boardheight = (int)boardt.sizeDelta.y;
        currstate = states.boardout;
        //fullstring = "Spiky Vroomer and its cohorts attacked!";
        fullstring = "";
        currstring = "";
        txt.text = "";
        txtsfx = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currstate) 
        {
            case states.boardout:
                break;
            case states.boardentering:
                txt.text = "";
                boardt.anchoredPosition = new Vector2(boardt.anchoredPosition.x, boardt.anchoredPosition.y - boardspeed);
                if (boardt.anchoredPosition.y <= 0)
                {
                    boardt.anchoredPosition = new Vector2(boardt.anchoredPosition.x, 0);
                    currstate = states.boardin;
                }
                break;
            case states.boardin:
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
                            if (currstring[currstring.Length - 1] == '.')
                            {
                                timer = 45;
                            }
                            else if (fullstring[currstring.Length - 1] == ',')
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
                    currboardtimer += Time.deltaTime;
                    if (currboardtimer >= boardtimer)
                    {
                        currstate = states.boardexiting;
                        currboardtimer = 0.0f;
                    }
                }
                break;
            case states.boardexiting:
                boardt.anchoredPosition = new Vector2(boardt.anchoredPosition.x, boardt.anchoredPosition.y + boardspeed);
                if (boardt.anchoredPosition.y >= boardheight)
                {
                    boardt.anchoredPosition = new Vector2(boardt.anchoredPosition.x, boardheight);
                    currstate = states.boardout;
                }
                break;
        }

        //if (Input.GetKeyDown("space"))
        //{
        //    teststring = teststring + " Test,";
        //    UpdateString(teststring);
        //}
    }


    public void UpdateString(string newtext)
    {
        currstate = states.boardentering;
        fullstring = newtext;
        charindex = 0;
        currboardtimer = 0.0f;
    }
}
