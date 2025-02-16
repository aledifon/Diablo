using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float speed;

    Vector3 offset;

    void Start()
    {
        // Initial distance between the camera and the player
        offset = transform.position - player.position;
    }
    
    void LateUpdate()
    {
        Vector3 targetCamPos = player.position + offset;

        transform.position = Vector3.Lerp(transform.position,targetCamPos,speed*Time.deltaTime);
    }
}
