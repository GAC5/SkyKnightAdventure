using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] Animator levelExit;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject self;
    public Collider2D bossBox;

    private void Start()
    {
        levelExit.SetBool("Boss Defeated", false);
        boss = GameObject.FindWithTag("Boss");
        bossBox = boss.GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (levelExit.GetBool("Boss Defeated") == false)
        {
            if (bossBox.isActiveAndEnabled == false)
            {
                levelExit.SetBool("Boss Defeated", true);
            }
        }
    }
}
