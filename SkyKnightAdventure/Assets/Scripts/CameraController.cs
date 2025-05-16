using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 cameraVelocity;
    [SerializeField] float smoothTime;
    [SerializeField] bool lookAtPlayer;
    [SerializeField] int lowerLimit = -2;
    [SerializeField] Camera camera;
    [SerializeField] GameObject bgNight;
    [SerializeField] GameObject bgDay;
    [SerializeField] GameObject mainCam;

    public static CameraController instance;

    private void FixedUpdate()
    {
        player = GameObject.Find("Hero").transform;
    }

    void Update()
    {
        if (player.position.y >= lowerLimit)
        {
            //transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraVelocity, smoothTime);
            if (lookAtPlayer)
            {
                transform.LookAt(player);
            }
        }
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (currentScene == totalScenes - 2)
        {
            Destroy(bgDay);
            bgNight.SetActive(true);
        }
        if (currentScene == totalScenes - 1)
        {
            Destroy(camera);
            Destroy(bgNight);
        }
    }
}
