using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceMenuScript : MonoBehaviour
{
	public GameObject ChoiceMenu;
	public Button YesButton;
	public Text Question;
	public PlayerMovement movscript;

    // Start is called before the first frame update
    void Start()
    {
        movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
     	if (ChoiceMenu.activeSelf && !movscript.GetMovementLock()) {
     		movscript.LockMovement();
     	}
    }

    public void openMenu(ItemDirectory.ItemIndex keyId, string q, Action a){
    	ChoiceMenu.SetActive(true);
    	Question.text = "Use a " + q + " to open the box?";
    	YesButton.onClick.AddListener(() => a());
    	movscript.LockMovement();
    }

    public void closeMenu(){
    	ChoiceMenu.SetActive(false);
    	// movscript.UnlockMovement();
    }
}
