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

    void Start()
    {
        lastYPos = transform.position.y;
    }

    //TODO: Figure out wtf causes 2hp to be deducted
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("PlayerStrike"))
        {
            health--;
            Debug.Log(health);
            HealthUpdate();
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
