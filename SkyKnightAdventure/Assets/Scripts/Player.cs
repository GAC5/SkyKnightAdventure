using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float jumpForce;
    [SerializeField] float speed;
    [SerializeField] bool grounded;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] float lastYPos;
    [SerializeField] int whichAttackAnim = 0;
    public int health = 3;
    public Animator animator;
    public bool attacking;
    public GameObject swordBox;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grounded = false;
        lastYPos = transform.position.y;
        animator.SetInteger("Health", health);
        swordBox.SetActive(false);
    }
    void Update()
    {
        if (health > 0)
        {
            CheckForJump();
            CheckMovement();
            SpriteFlip();
            CheckAttack();
            SwordBoxMove();
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

    void SwordBoxMove()
    {
        if (renderer.flipX)
        {
            swordBox.transform.position = new Vector2(transform.position.x -1, swordBox.transform.position.y);
        }
        else
        {
            swordBox.transform.position = new Vector2(transform.position.x + 1, swordBox.transform.position.y);
        }
    }

    void CheckForJump ()
    {
        if (grounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    void CheckMovement()
    {
        float leftRight = Input.GetAxis("Horizontal");
        transform.Translate(leftRight * Time.deltaTime * speed, 0, 0);
        if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow)))))
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
        if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            renderer.flipX = true;
        }
        if ((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)))
        {
            renderer.flipX = false;
        }
    }
    
    void CheckAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (whichAttackAnim > 2)
            {
                whichAttackAnim = 0;
            }
            animator.SetTrigger("Attack");
            animator.SetInteger("WhichAttackAnim", whichAttackAnim);
            whichAttackAnim++;
        }
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
            health--;
        }
    }

    void Restart()
    {
        string currentscene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentscene);
    }
}

