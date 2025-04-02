using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] Renderer renderer;
    [SerializeField] Animator animator;
    [SerializeField] Transform player;
    [SerializeField] int healthValue;
    [SerializeField] int attackValue;
    [SerializeField] float detectionDistance;
   

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDetect();
    }

    private void PlayerDetect()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) <= detectionDistance)
        {
            
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

    private void EnemyDead()
    {
        Destroy(gameObject);
    }

}
