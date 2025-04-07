using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject portal;
    public Player playerScript;
    
    void Start()
    {
        playerScript = GameObject.Find("Hero").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                playerScript.transform.position = portal.transform.position;
            }
        }
    }
}
