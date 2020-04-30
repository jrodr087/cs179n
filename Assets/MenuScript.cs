using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject itemMenu;
    public GameObject statusMenu;
    public GameObject equipMenu;
    private int currNest = 0;
    private GameObject openMenu;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        itemMenu.SetActive(false);
        statusMenu.SetActive(false);
        equipMenu.SetActive(false);
        audio = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleItemMenu()
    {
        if (currNest == 0 && !itemMenu.activeSelf)
        {
            itemMenu.SetActive(true);
            currNest = 1;
            openMenu = itemMenu;
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
        }
        else if (currNest == 1 && itemMenu.activeSelf)
        {
            itemMenu.SetActive(false);
            currNest = 0;
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuClose"));
        }
        else {
            openAndClose(itemMenu, openMenu);
        }
    }
    public void ToggleStatusMenu()
    {
        if (currNest == 0 && !statusMenu.activeSelf)
        {
            statusMenu.SetActive(true);
            currNest = 1;
            openMenu = statusMenu;
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
        }
        else if (currNest == 1 && statusMenu.activeSelf)
        {
            statusMenu.SetActive(false);
            currNest = 0;
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuClose"));
        }
        else {
            openAndClose(statusMenu, openMenu);
        }
    }
    public void ToggleEquipMenu()
    {
        if (currNest == 0 && !equipMenu.activeSelf)
        {
            equipMenu.SetActive(true);
            currNest = 1;
            openMenu = equipMenu;
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
        }
        else if (currNest == 1 && equipMenu.activeSelf)
        {
            equipMenu.SetActive(false);
            currNest = 0;
            audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuClose"));
        }
        else {
            openAndClose(equipMenu, openMenu);
        }
    }

    private void openAndClose(GameObject toOpen, GameObject toClose){
        toClose.SetActive(false);
        currNest = 1;
        openMenu = toOpen;
        toOpen.SetActive(true);
        audio.PlayOneShot((AudioClip)Resources.Load("Sounds/SecondaryMenuOpen"));
    }
}
