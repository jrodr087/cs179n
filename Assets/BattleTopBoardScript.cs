using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTopBoardScript : MonoBehaviour
{
    public Text txt;
    public Image board;
    private int boardspeed = 10;
    private int boardheight = 128;
    private RectTransform boardt;
    private List<string> stringlist = new List<string>();
    private enum states {boardout,boardentering,boardin,boardexiting};
    private states currstate = states.boardout;
    private int timer = 0;
    private int textspeed = 1;
    private float boardtimer = 3.0f;
    private float currboardtimer = 0.0f;
    private float stringtimer = 1.0f;
    private float currstringtimer = 0.0f;
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
                        charindex+= 2;
                        if (charindex > fullstring.Length) { charindex = fullstring.Length; }
                        currstring = fullstring.Substring(0, charindex);
                        timer = textspeed;
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
                    if (stringlist.Count > 0 && currstringtimer >= stringtimer)
                    {
                        currboardtimer = 0.0f;
                        currstringtimer = 0.0f;
                        currstring = "";
                        charindex = 0;
                        fullstring = stringlist[0];
                        stringlist.RemoveAt(0);
                    }
                    currboardtimer += Time.deltaTime;
                    currstringtimer += Time.deltaTime;
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

    }


    public void UpdateString(string newtext)
    {
        if (currstate != states.boardin && currstate != states.boardentering)
        {
            currstate = states.boardentering;
            charindex = 0;
            fullstring = newtext;
        }
        else
        {
            stringlist.Add(newtext);
        }
        currboardtimer = 0.0f;
    }
}
