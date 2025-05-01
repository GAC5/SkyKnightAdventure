using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] Animator levelExit;
    [SerializeField] GameObject miniBoss;
    public Collider2D bossCollider;

    private void Start()
    {
        bossCollider = miniBoss.GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (!bossCollider.isActiveAndEnabled)
        {
            levelExit.SetBool("Boss Defeated", true);
        }
    }
}
