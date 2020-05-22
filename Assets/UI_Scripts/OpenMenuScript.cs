using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement movscript;
    public GameObject panel;
    AudioSource audio;
    public bool active = false;
    void Start()
    {
        panel.SetActive(false);
        audio = gameObject.AddComponent<AudioSource>(); 
        movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!movscript.GetMovementLock() && !active)
            {
                panel.SetActive(true);
                active = true;
                movscript.LockMovement();
                audio.PlayOneShot((AudioClip)Resources.Load("Sounds/MenuOpen"));
            }
            else if (movscript.GetMovementLock() && active)
            {
                panel.SetActive(false);
                active = false;
                movscript.UnlockMovement();
            }
        }
        if ( active && !movscript.GetMovementLock()){
            movscript.LockMovement();
        }
    }
}
