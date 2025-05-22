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
    [SerializeField] Collider2D collider;
    private GameObject player;

    [SerializeField] int healthValue;
    [SerializeField] int attackValue;
    [SerializeField] float detectionDistanceX;
    [SerializeField] float detectionDistanceY;
    [SerializeField] float attackRange;
    [SerializeField] float verticalAttackThreshold;
    [SerializeField] float speed;
    [SerializeField] float movementThreshold = 0.01f;
    [SerializeField] float attackCooldown;
    [SerializeField] bool roamingActivated;
    [SerializeField] float roamingRange;
    [SerializeField] float roamingSpeed;
    [SerializeField] bool roamLeftStart;
    [SerializeField] bool roamWait;
    [SerializeField] float minIdlePauseTime;
    [SerializeField] float maxIdlePauseTime;
    private float distanceToPlayerX;
    private float distanceToPlayerY;
    private float enemyLastXPosition;
    private Vector3 originalScale;
    private float lastAttackTime;
    private bool enemyDead;
    private float leftRoamLimit;
    private float rightRoamLimit;
    private float startXPosition;
    private bool isPausing;




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Hero");
        if (player != null)
        {
            player.GetComponent<Transform>();
        }
        enemyLastXPosition = transform.position.x;
        originalScale = transform.localScale;
        startXPosition = transform.position.x;
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

    //   transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    private void PlayerDetect()
    {
        distanceToPlayerX = Mathf.Abs(transform.position.x - player.transform.position.x);
        distanceToPlayerY = Mathf.Abs(transform.position.y - player.transform.position.y);
        if ((distanceToPlayerX <= detectionDistanceX) && (distanceToPlayerY <= detectionDistanceY) && (distanceToPlayerX > attackRange))
        {
            if (!animator.GetBool("isStationary"))
            {
                MoveTowardsPlayer();
            }
            CheckForMove();
        }
        else if ((distanceToPlayerX <= detectionDistanceX) && (distanceToPlayerY <= detectionDistanceY) && (distanceToPlayerX <= attackRange))
        {
            EnemyAttack();
            CheckForMove();

        }
        else
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            if (roamingActivated)
            {
                if (!animator.GetBool("isStationary"))
                {
                    idleWalk();
                }
            }

        }
    }

    private void idleWalk()
    {
        leftRoamLimit = startXPosition - roamingRange;
        rightRoamLimit = startXPosition + roamingRange;

        if (roamLeftStart)
        {
            if (transform.position.x > leftRoamLimit)
            {
                rigidbody.velocity = new Vector2(-roamingSpeed, rigidbody.velocity.y);
            }
            else if (transform.position.x <= leftRoamLimit)
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                if (roamWait)
                {
                    if (!isPausing)
                    {
                        StartCoroutine(PauseAtLimit());
                    }
                }
                else
                {
                    roamLeftStart = false;
                }
            }
        }
        if (!roamLeftStart)
        {
            if (transform.position.x < rightRoamLimit)
            {
                rigidbody.velocity = new Vector2(roamingSpeed, rigidbody.velocity.y);
            }
            else if (transform.position.x >= rightRoamLimit)
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                if (roamWait)
                {
                    if (!isPausing)
                    {
                        StartCoroutine(PauseAtLimit());
                    }
                }
                else
                {
                    roamLeftStart = true;
                }
            }
        }
        CheckForMoveIdle();
    }

    private IEnumerator PauseAtLimit()
    {
        isPausing = true;
        float waitTime = Random.Range(minIdlePauseTime, maxIdlePauseTime);
        yield return new WaitForSeconds(waitTime);
        if (roamLeftStart)
        {
            roamLeftStart = false;
        }
        else if (!roamLeftStart)
        {
            roamLeftStart = true;
        }
        isPausing = false;
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
    }

    private void CheckForMove()
    {
        float enemyMovement = transform.position.x - enemyLastXPosition;

        if (distanceToPlayerX <= attackRange)
        {
            if ((transform.position.x - player.transform.position.x) < -.1)
            {
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else if ((transform.position.x - player.transform.position.x) > .1)
            {
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
        }
        else
        {
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
                    transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
                }
                else if (enemyMovement > 0)
                {
                    transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
                }
            }

            enemyLastXPosition = transform.position.x;
        }

    }

    private void CheckForMoveIdle()
    {
        float enemyMovement = transform.position.x - enemyLastXPosition;

        bool isMoving = Mathf.Abs(enemyMovement) > movementThreshold;

        if (isMoving == true && animator.GetBool("runAnimationController") == false)
        {
            animator.SetTrigger("enemyIdle");
            animator.SetBool("runAnimationController", true);
        }

        if (isMoving)
        {

            if (enemyMovement < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else if (enemyMovement > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
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
                animator.SetBool("isStationary", true);
                animator.SetTrigger("enemyAttack");
                lastAttackTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyDead == false)
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
                    enemyDead = true;
                    StartCoroutine(MakeSureDead());
                }
            }
        }
    }

    private IEnumerator MakeSureDead()
    {
        yield return new WaitForSeconds(4);
        if (enemyDead == true)
        {
            animator.SetTrigger("enemyDead");
            StartCoroutine(MakeSureDead());
        }
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
