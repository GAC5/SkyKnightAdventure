using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] Renderer renderer;
    [SerializeField] Animator animator;
    [SerializeField] int healthValue;
    [SerializeField] int attackValue;
    [SerializeField] bool enemyDead;
   

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        CheckForEnemyDead();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerStrike"))
        {
            if (healthValue > 1)
            {
                healthValue--;
            }
            else if (healthValue <= 1)
            {
                healthValue--;
                enemyDead = true;
            }
        }
    }

    private void CheckForEnemyDead()
    {
        if (enemyDead == true)
        {
            Destroy(gameObject);
        }
    }
}
