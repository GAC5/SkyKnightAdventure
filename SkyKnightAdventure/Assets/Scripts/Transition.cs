using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] Animator levelExit;
    [SerializeField] GameObject boss;
    public Animator bossAnim;

    private void Start()
    {
        boss = GameObject.FindWithTag("Boss");
        bossAnim = boss.GetComponent<Animator>();
    }

    private void Update()
    {
        if (bossAnim != null)
        {
                levelExit.SetBool("Boss Defeated", bossAnim.GetBool("enemyDead"));
        }    
    }
}
