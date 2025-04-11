using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] Renderer renderer;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] Transform player;
    [SerializeField] int healthValue;
    [SerializeField] int attackValue;
    [SerializeField] float detectionDistanceX;
    [SerializeField] float detectionDistanceY;
    private float distanceToPlayerX;
    private float distanceToPlayerY;
    [SerializeField] float attackRange;
    [SerializeField] float verticalAttackThreshold;
    [SerializeField] float speed;
    [SerializeField] float movementThreshold = 0.01f;
    [SerializeField] float attackCooldown;
    private float enemyLastXPosition;
    private bool runAnimationController;
    private float lastAttackTime;


   

    // Start is called before the first frame update
    void Start()
    {
        enemyLastXPosition = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        PlayerDetect();
        //CheckForMove();
    }

    //   transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    private void PlayerDetect()
    {
        distanceToPlayerX = Mathf.Abs(transform.position.x - player.position.x);
        distanceToPlayerY = Mathf.Abs(transform.position.y - player.position.y);
        if ((distanceToPlayerX <= detectionDistanceX) && (distanceToPlayerY <= detectionDistanceY) && (distanceToPlayerX > attackRange))
        {
            MoveTowardsPlayer();
        }
        else if ((distanceToPlayerX <= detectionDistanceX) && (distanceToPlayerY <= detectionDistanceY) && (distanceToPlayerX <= attackRange))
        {
            EnemyAttack();
            
        }
        else
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            // this is the section where no player is detected
            //later, a passive animation or walk could be added to make enemies more dynamic
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rigidbody.velocity = new Vector2(direction.x * speed, rigidbody.velocity.y);
        CheckForMove();
        
    }

    private void CheckForMove()
    {
        float enemyMovement = transform.position.x - enemyLastXPosition;

        bool isMoving = Mathf.Abs(enemyMovement) > movementThreshold;

        if (isMoving == true && animator.GetBool("runAnimationController") == false)
        {
            animator.SetTrigger("enemyMoving");
            animator.SetBool("runAnimationController", true);
        }

        if (isMoving)
        {
            if (enemyMovement < 0)
            {
                transform.localScale = new Vector3(1.65f, 1.65f, 1.65f);
            }
            else if (enemyMovement > 0)
            {
                transform.localScale = new Vector3(-1.65f, 1.65f, 1.65f);
            }
        }

        enemyLastXPosition = transform.position.x;
    }

    private void EnemyAttack()
    {
        if ((distanceToPlayerX <= attackRange) && (distanceToPlayerY <= verticalAttackThreshold))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                animator.SetTrigger("enemyAttack");
                lastAttackTime = Time.time;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerStrike"))
        {
            if (healthValue > 1)
            {
                animator.SetTrigger("isTakingDamage");
                healthValue--; 
            }
            else if (healthValue <= 1)
            {
                healthValue--;
                animator.SetTrigger("enemyDead");
            }
        }
    }

    private void AnimationController()
    {
        if (animator.GetBool("runAnimationController") == true) 
        {
            animator.SetBool("runAnimationController", false);
        }
    }

    private void EnemyDead()
    {
        Destroy(gameObject);
    }

}
