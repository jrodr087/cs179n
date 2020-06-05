using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public Animator maleAnimator;
    public Animator femaleAnimator;
    public GameObject maleSprite;
    public GameObject femaleSprite;

    public GameObject battle;
    public bool levelled = false;
    private bool movementlocked = false;
    private LevelUpNotificationScript lvluppanel;
    private PlayerData pd;
    Vector2 lastmovement;
    Vector2 movement;
    void Start()
    {
        lvluppanel = GameObject.Find("UI/Level Up Panel").GetComponent<LevelUpNotificationScript>();
        pd = gameObject.GetComponent<PlayerData>();
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
            if (levelled)
            {
                levelled = false;
                lvluppanel.StartNotification();
            }
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
        if (pd.male)
        {
            animator = maleAnimator;
            femaleSprite.SetActive(false);
            maleSprite.SetActive(true);
        }
        else
        {
            animator = femaleAnimator;
            maleSprite.SetActive(false);
            femaleSprite.SetActive(true);
        }
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
