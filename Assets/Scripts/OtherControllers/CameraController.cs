using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private Transform player; // Mario's Transform
    private Transform endLimit; // GameObject that indicates end of map

    private Vector3 startPosition;
    private float startX; // smallest x-coordinate of the Camera
    private float offset; // initial x-offset between camera and Mario
    private float viewportHalfWidth;
    private float endX; // largest x-coordinate of the camera


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        endLimit = GameObject.Find("EndLimit").transform;

        startPosition = this.transform.position;
        startX = this.transform.position.x;
        offset = this.transform.position.x - player.position.x;
        // get coordinate of the bottomleft of the viewport. z doesn't matter since the camera is orthographic
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)); // the z-component is the distance of the resulting plane from the camera 
        viewportHalfWidth = Mathf.Abs(bottomLeft.x - this.transform.position.x);

        endX = endLimit.transform.position.x - viewportHalfWidth;
    }

    void Update()
    {
        float desiredX = player.position.x + offset;
        // check if desiredX is within startX and endX
        if (desiredX > startX && desiredX < endX)
        {
            this.transform.position = new Vector3(desiredX, this.transform.position.y, this.transform.position.z);
        }
    }

    public void OnLevelRestart()
    {
        // reset camera position
        transform.position = startPosition;
    }
}
