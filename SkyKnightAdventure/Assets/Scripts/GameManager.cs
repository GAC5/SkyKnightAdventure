using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverCanvas;
    public Player playerScript;

    void Start()
    {
        playerScript = GameObject.Find("Hero").GetComponent<Player>();
    }

    void Update()
    {
        HealthMonitor();   
    }

    void HealthMonitor()
    {
        if (playerScript.health < 1)
        {
            gameOverCanvas.SetActive(true);
        }
    }

    public void Restart()
    {
        string currentscene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentscene);
    }

}
