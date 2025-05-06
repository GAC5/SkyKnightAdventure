using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 cameraVelocity;
    [SerializeField] float smoothTime;
    [SerializeField] bool lookAtPlayer;
    [SerializeField] int lowerLimit = -2;
    [SerializeField] Camera camera;

    public static CameraController instance;

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
    }
}
