using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private bool activated = false;
    public UnityEvent eventToTrigger;
    public AudioClip sound;
    new AudioSource audio;
    private PlayerMovement movscript;
    private bool inside = false;
    // Start is called before the first frame update
    void Start()
    {
        movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        audio = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && activated == false && !movscript.GetMovementLock())
        {
            activated = true;
            eventToTrigger.Invoke();
            audio.PlayOneShot(sound);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
    }
}
