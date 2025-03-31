using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class DamageDetector : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] int health = 3;
    [SerializeField] float lastYPos;
    private int triggered;

    void Start()
    {
        lastYPos = transform.position.y;
    }

    private void FixedUpdate()
    {
        HealthUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerStrike"))
        {
            //PROBLEM: Trigger called twice.
            //SOLUTION: if/else statement checks how many times trigger is called, if triggered more than once, no health is deducted.
            triggered++;
            if (triggered == 1)
            {
                health--;
            }
            else
            {
                triggered = 0;
            }

        }
    }

    void HealthUpdate()
    {
        if (health != animator.GetInteger("Health"))
        {
            animator.SetTrigger("Hurt");
        }
        animator.SetInteger("Health", health);
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
