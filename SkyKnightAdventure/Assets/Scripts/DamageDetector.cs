using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class DamageDetector : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] int enemyHealth = 3;
    [SerializeField] float lastYPos;

    void Start()
    {
        lastYPos = transform.position.y;
    }

    private void FixedUpdate()
    {
        HealthUpdate();
    }

    //TODO: Figure out wtf causes 2hp to be deducted
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerStrike"))
        {
            enemyHealth--;
            Debug.Log(enemyHealth);
        }
    }

    void HealthUpdate()
    {
        if (enemyHealth != animator.GetInteger("Health"))
        {
            animator.SetTrigger("Hurt");
        }
        animator.SetInteger("Health", enemyHealth);
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
