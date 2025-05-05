using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;



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

    public void Restart()
    {
        string currentscene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentscene);
    }
    
    public void NextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }
}
