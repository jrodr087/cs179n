using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectScript : MonoBehaviour
{
    public string[] interactableDialogue;
    private CutsceneScript handler;
    private bool inside = false;
    // Start is called before the first frame update
    void Start()
    {
        handler = GameObject.Find("/UI/VignetteController").GetComponent<CutsceneScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && inside)
        {
            handler.StartScene(interactableDialogue);
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
