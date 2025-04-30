using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float jumpForce;
    [SerializeField] float speed;
    [SerializeField] bool grounded;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] float lastYPos;
    [SerializeField] int whichAttackAnim = 0;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject heart3;
    [SerializeField] GameObject heart2;
    [SerializeField] GameObject heart1;
    [SerializeField] float attackCooldown;
    public int health = 3;
    public Animator animator;
    public bool attacking;
    public GameObject swordBox;
    private float lastAttackTime;


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
        }
        HealthUpdate();
    }

    private void FixedUpdate()
    {
        if (health > 0)
        {
            CheckForFalling();
        }
        grounded = animator.GetBool("Grounded");
    }

    void CheckForJump ()
    {
        if (grounded && ((Input.GetKeyDown(KeyCode.W))))
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    void CheckMovement()
    {
        if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
        {
            float leftRight = Input.GetAxis("Horizontal");
            transform.Translate(leftRight * Time.deltaTime * speed, 0, 0);
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
        if (collision.transform.CompareTag("Ground"))
        {
            animator.SetBool("Grounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            animator.SetBool("Grounded", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            health = 0;
        }
        
        if (collision.CompareTag("EnemyStrike"))
        {
            health--;
        }

        if (collision.CompareTag("Finish"))
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int totalScenes = SceneManager.sceneCountInBuildSettings;
            if (currentScene == totalScenes - 1)
            {
            }
            SceneManager.LoadScene(currentScene + 1);
        }
    }

    void SpriteFlip()
    {
        if ((Input.GetKey(KeyCode.A)))
        {
            transform.localScale = new Vector2 (-1.5f, 1.5f);
        }
        if ((Input.GetKey(KeyCode.D)))
        {
            transform.localScale = new Vector2(1.5f, 1.5f);
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
            /*if (Time.time >= lastAttackTime + attackCooldown)
            {
                if (whichAttackAnim > 2)
                {
                    whichAttackAnim = 0;
                }
                animator.SetTrigger("Attack");
                animator.SetInteger("WhichAttackAnim", whichAttackAnim);
                whichAttackAnim++;
            }*/
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
        if (health < 1)
        {
            gameOverCanvas.SetActive(true);
        }
        switch (health)
        {
            case 2:
                heart3.SetActive(false);
                break;
            case 1:
                heart3.SetActive(false);
                heart2.SetActive(false);
                break;
            case < 1:
                heart3.SetActive(false);
                heart2.SetActive(false);
                heart1.SetActive(false);
                break;
        }

    }

    void Restart()
    {
        string currentscene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentscene);
    }
}

