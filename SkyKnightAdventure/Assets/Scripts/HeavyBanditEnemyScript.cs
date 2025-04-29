using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBanditEnemyScript : MonoBehaviour
{
    [SerializeField] Renderer renderer;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] Collider2D collider;
    private GameObject player;
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
    private float lastAttackTime;
    private bool enemyDead;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.GetComponent<Transform>();
        }
        enemyLastXPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyDead == false)
        {
            PlayerDetect();
        }
        else
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            rigidbody.gravityScale = 0;
            collider.enabled = false;

        }
    }

    private void PlayerDetect()
    {
        distanceToPlayerX = Mathf.Abs(transform.position.x - player.transform.position.x);
        distanceToPlayerY = Mathf.Abs(transform.position.y - player.transform.position.y);
        if ((distanceToPlayerX <= detectionDistanceX) && (distanceToPlayerY <= detectionDistanceY) && (distanceToPlayerX > attackRange))
        {
            MoveTowardsPlayer();
        }
        else if ((distanceToPlayerX <= detectionDistanceX) && (distanceToPlayerY <= detectionDistanceY) && (distanceToPlayerX <= attackRange))
        {
            //EnemyAttack();

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
        Vector2 direction = (player.transform.position - transform.position).normalized;

        if (animator.GetBool("isStationary") == false)
        {
            rigidbody.velocity = new Vector2(direction.x * speed, rigidbody.velocity.y);
        }
        else
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }

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
                transform.localScale = new Vector3(2f, 2f, 2f);
            }
            else if (enemyMovement > 0)
            {
                transform.localScale = new Vector3(-2f, 2f, 2f);
            }
        }

        enemyLastXPosition = transform.position.x;
    }




    private void AnimationRunController()
    {
        if (animator.GetBool("runAnimationController") == true)
        {
            animator.SetBool("runAnimationController", false);
        }
    }

    private void AnimationStationaryController()
    {
        if (animator.GetBool("isStationary") == true)
        {
            animator.SetBool("isStationary", false);
        }
    }

    private void AnimationHurtResetController()
    {
        if (animator.GetBool("runAnimationController") == true)
        {
            animator.SetBool("runAnimationController", false);
        }
        if (animator.GetBool("isStationary") == true)
        {
            animator.SetBool("isStationary", false);
        }
    }

    private void EnemyDead()
    {
        Destroy(gameObject);
    }

}


