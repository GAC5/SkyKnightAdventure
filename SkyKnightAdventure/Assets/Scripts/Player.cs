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
    [SerializeField] GameObject hero;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float jumpForce;
    [SerializeField] float speed;
    [SerializeField] bool grounded;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] float lastYPos;
    [SerializeField] int whichAttackAnim = 0;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject cooldownSprite;
    [SerializeField] Text healthText;
    [SerializeField] float attackCooldown;
    [SerializeField] int startHealth;
    [SerializeField] float startAttackCooldown;
    [SerializeField] GameObject upgradeMenu;
    public Vector3 startPos = new Vector3(-10, -5, 0);
    public int health = 3;
    public Animator animator;
    public bool attacking;
    public GameObject swordBox;
    private float lastAttackTime;

    public static Player instance;

    private void Awake()
    {
        //if no gamemanager / if not original gamemanager
        if (instance != null && instance != this)
        {
            //destroy duplicate
            Destroy(gameObject);

            //ensures the rest of the awake method doesnt execute
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        upgradeMenu.SetActive(false);
        startHealth = health;
        startAttackCooldown = attackCooldown;
        rb = GetComponent<Rigidbody2D>();
        grounded = false;
        lastYPos = transform.position.y;
        animator.SetInteger("Health", health);
        swordBox.SetActive(false);
    }

    private void Update()
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
            upgradeMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void UpgradeHealth()
    {
        health = health + 5;
        Time.timeScale = 1;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (currentScene == totalScenes - 1)
        {
        }
        SceneManager.LoadScene(currentScene + 1);
        startPos = hero.transform.position;
        startHealth = health;
        startAttackCooldown = attackCooldown;
        upgradeMenu.SetActive(false);
    }

    public void UpgradeCooldown()
    {
        attackCooldown = attackCooldown - 0.2f;
        Time.timeScale = 1;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (currentScene == totalScenes - 1)
        {
        }
        SceneManager.LoadScene(currentScene + 1);
        startPos = hero.transform.position;
        startHealth = health;
        startAttackCooldown = attackCooldown;
        upgradeMenu.SetActive(false);
    }

    public void UpgradeSpeed()
    {
        speed = speed + 2;
        Time.timeScale = 1;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (currentScene == totalScenes - 1)
        {
        }
        SceneManager.LoadScene(currentScene + 1);
        startPos = hero.transform.position;
        startHealth = health;
        startAttackCooldown = attackCooldown;
        upgradeMenu.SetActive(false);
    }

    public void UpgradeJump()
    {
        jumpForce = jumpForce + 2;
        Time.timeScale = 1;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (currentScene == totalScenes - 1)
        {
        }
        SceneManager.LoadScene(currentScene + 1);
        startPos = hero.transform.position;
        startHealth = health;
        startAttackCooldown = attackCooldown;
        upgradeMenu.SetActive(false);
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
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            cooldownSprite.SetActive(false);
            if (Input.GetMouseButton(0))
            {
                if (whichAttackAnim > 2)
                {
                    whichAttackAnim = 0;
                }
                animator.SetTrigger("Attack");
                animator.SetInteger("WhichAttackAnim", whichAttackAnim);
                whichAttackAnim++;
                lastAttackTime = Time.time;
            }
        }
        else
        {
            cooldownSprite.SetActive(true);
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
        healthText.text = health.ToString();
    }

    public void Restart()
    {
        hero.transform.position = startPos;
        health = startHealth;
        attackCooldown = startAttackCooldown;
        Debug.Log("Restart");
        string currentscene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentscene);
        gameOverCanvas.SetActive(false);
    }
}

