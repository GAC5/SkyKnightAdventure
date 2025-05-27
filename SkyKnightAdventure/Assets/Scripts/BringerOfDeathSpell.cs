using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BringerOfDeathSpell : MonoBehaviour
{
    private GameObject boss;
    [SerializeField] Animator animator;
    [SerializeField] Renderer renderer; 

    [SerializeField] float hoverSpeed;
    public float hoverHeight;
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("BringerOfDeath");
        if (boss != null)
        {
            //boss.GetComponent<Transform>();
        }
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToBossLocation();
    }

    private void MoveToBossLocation()
    {
        float newX = Mathf.Lerp(transform.position.x, boss.transform.position.x, hoverSpeed * Time.deltaTime);
        float newY = Mathf.Lerp(transform.position.y, boss.transform.position.y + hoverHeight, hoverSpeed * Time.deltaTime);
        transform.position = new Vector2(newX, newY);
    }

    public void BossDead()
    {
        transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        animator.SetTrigger("bossDead");
    }

    private void EnemyDead()
    {
        Destroy(transform.parent.gameObject);
    }
    
}