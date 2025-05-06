using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

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
}
