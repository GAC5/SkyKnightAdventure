using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float jumpForce;
    [SerializeField] float speed;
    [SerializeField] bool grounded;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] int health = 3;
    [SerializeField] float lastYPos;
    public bool block;
    public Animator animator;
    public bool attacking;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grounded = false;
        lastYPos = transform.position.y;
        animator.SetInteger("Health", health);
    }
    void Update()
    {
        if (health > 0)
        {
            CheckForJump();
            CheckMovement();
            SpriteFlip();
            CheckAttack();
            CheckBlock();
        }
        HealthUpdate();
    }

    private void FixedUpdate()
    {
        if (health > 0)
        {
            CheckForFalling();
        }
    }

    void CheckForJump ()
    {
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    void CheckMovement()
    {
        float leftRight = Input.GetAxis("Horizontal");
        transform.Translate(leftRight * Time.deltaTime * speed, 0, 0);
        if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.RightArrow)))
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    void CheckForFalling()
    {
        if (transform.position.y >= lastYPos)
        {
            animator.SetBool("Falling", false);
        }
        else
        {
            animator.SetBool("Falling", true);
        }
        lastYPos = transform.position.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Ground"))
        {
            grounded = true;
            animator.SetBool("Grounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            grounded = false;
            animator.SetBool("Grounded", false);
        }
    }
    void SpriteFlip()
    {
        if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            renderer.flipX = true;
        }
        if ((Input.GetKeyDown(KeyCode.LeftArrow)) || (Input.GetKeyDown(KeyCode.D)))
        {
            renderer.flipX = false;
        }
    }

    void CheckAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack");
        }
    }

    void CheckBlock()
    {
        if ((Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow)))
        {
            if (grounded && !animator.GetBool("Moving"))
            animator.SetBool("Block", true);
        }
        if (!grounded || animator.GetBool("Moving") || (Input.GetKeyUp(KeyCode.S)) || (Input.GetKeyUp(KeyCode.DownArrow)))
        {
            animator.SetBool("Block", false);
        }
        block = animator.GetBool("Block");
    }

    void HealthUpdate()
    {
        if (health != animator.GetInteger("Health"))
        {
            animator.SetTrigger("Hurt");
        }
        animator.SetInteger("Health", health);
        if (Input.GetKeyDown(KeyCode.K))
        {
            health --;
        }
    }
}

