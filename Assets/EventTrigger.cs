using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lowscope;
using Lowscope.Saving;

[System.Serializable]
public class EventTrigger : MonoBehaviour, ISaveable
{
    // Start is called before the first frame update
    private bool activated = false;
    public UnityEvent eventToTrigger;
    public AudioClip sound;
    new AudioSource audio;
    public bool continous;
    private PlayerMovement movscript;

    [System.Serializable]
    public struct EventData
    {
        public bool activate;
    }

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
            if (!continous){
                activated = true;
            }
            eventToTrigger.Invoke();
            audio.PlayOneShot(sound);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
    }

    [SerializeField]
    private EventData eventData;

    public string OnSave()
        {
            return JsonUtility.ToJson((new EventData() { activate = activated }));
        }

        public void OnLoad(string data)
        {
            //stats = JsonUtility.FromJson<PlayerStats>(data);
            eventData = JsonUtility.FromJson<EventData>(data);
            activated = eventData.activate;
        }

        public bool OnSaveCondition()
        {
            return true;
        }
}
