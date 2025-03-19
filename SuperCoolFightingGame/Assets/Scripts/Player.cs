using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float jumpForce;
    [SerializeField] float speed;
    [SerializeField] bool grounded;
    [SerializeField] Animator animator;
    private float lastYPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grounded = false;
        lastYPos = transform.position.y;
    }
    void Update()
    {
        CheckForJump();
        CheckMovement();
    }

    private void FixedUpdate()
    {
        CheckForFalling();
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

        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x * -speed * Time.deltaTime, transform.position.y, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x * speed * Time.deltaTime, transform.position.y, 0);
        }

        if ((Input.GetKey(KeyCode.A)||(Input.GetKey(KeyCode.D))))
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
}

