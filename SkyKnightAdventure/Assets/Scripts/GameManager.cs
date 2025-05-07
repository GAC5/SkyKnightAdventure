using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool restart;
    public bool next;

    private void Awake()
    {
        //if no gamemanager / if not original gamemanager
        if (instance != null && instance != this)
        {
            //destroy duplicate
            Destroy(gameObject);

            //ensures the rest of the awake method doesnt execute
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        restart = false;
        next = false;
    }

    public void Restart()
    {
        restart = true;
        string currentscene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentscene);
        restart = false;
    }
    
    public void NextScene()
    {
        next = true;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
        next = false;
    }
}
