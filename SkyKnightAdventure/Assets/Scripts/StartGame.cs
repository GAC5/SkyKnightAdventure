using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Player playerScript;
    public GameObject player;

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Hero");
            playerScript = player.GetComponent<Player>();
        }
    }

    public void IronStart()
    {
        playerScript.IronStart();
        playerScript.hero.SetActive(true);
    }

    public void Begin()
    {
        playerScript.Begin();
        playerScript.hero.SetActive(true);
    }
}
