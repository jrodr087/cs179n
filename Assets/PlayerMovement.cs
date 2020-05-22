using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
	public float moveSpeed = 5f;
	public Rigidbody2D rb;
    public Animator animator;
    public GameObject battle;
    private bool movementlocked = false;
    Vector2 lastmovement;
	Vector2 movement;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {//input
        if (movementlocked)
        {
            movement.x = 0;
            movement.y = 0;
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        if (movement.magnitude > .1)
        {
            lastmovement = movement;
        }
        Animate();
	}
	
	void FixedUpdate()
	{//movement
		rb.MovePosition(rb.position+movement.normalized*moveSpeed*Time.fixedDeltaTime);
	}

	void ProcessInputs()
    {

    }

	void Animate()
    {
        animator.SetFloat("Horizontal", lastmovement.x);
        animator.SetFloat("Vertical", lastmovement.y);
        animator.SetFloat("Speed", movement.magnitude);
    }
    public void LockMovement()
    {
        movementlocked = true;
    }
    public void UnlockMovement()
    {
        movementlocked = false;
    }
    public bool GetMovementLock()
    {
        return movementlocked;
    }
    public void LeaveBattle()
    {
        CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        cs.Battle = null;
        cs.sub = CameraSubject.player;
        Destroy(battle);
    }
}
