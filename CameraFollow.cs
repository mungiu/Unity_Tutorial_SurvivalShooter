using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //since it is public we will associate player with is as "target"
    public Transform target;
    //smoothing the camera follow
    public float smoothing = 5f;

    Vector3 offset;

    private void Start()
    {
        //setting the distance between player and camera
        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        //calculating desired camera position according to constant distance
        Vector3 targetCamPosition = target.position + offset;
        //smoothly moving the camera to new position PER SECOND (Time.deltaTime) [since Fixed update goes 50 time per second]
        transform.position = Vector3.Lerp(transform.position, targetCamPosition, smoothing * Time.deltaTime);
    }
}
