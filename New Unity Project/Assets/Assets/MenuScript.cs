using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject itemlist;
    public GameObject status;
    private int currNest = 0;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        itemlist.SetActive(false);
        status.SetActive(false);
        audio = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleItemMenu()
    {
        if (currNest == 0 && !itemlist.activeSelf)
        {
            itemlist.SetActive(true);
            currNest = 1;
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
        }
        else if (currNest == 1 && itemlist.activeSelf)
        {
            itemlist.SetActive(false);
            currNest = 0;
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuClose"));
        }
    }
    public void ToggleStatusMenu()
    {
        if (currNest == 0 && !status.activeSelf)
        {
            status.SetActive(true);
            currNest = 1;
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
        }
        else if (currNest == 1 && status.activeSelf)
        {
            status.SetActive(false);
            currNest = 0;
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuClose"));
        }
    }
}
