using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float movementSpeed = 1;
    public float mouseSensitivity = 1;
    public float scrollSpeed = 1;
    public float scrollSmoothSpeed = 1;

    public float minHeight;
    public float maxHeight;
    public float heightLerp = 0.5f;

    private float smoothedHeight = 10;

    public Camera cam;

    void Update()
    {
        GetMovement();
        GetMouseInput();
        GetHeight();
    }

    private void GetMovement()
    {
        Vector3 move = Vector3.zero;
        move.x = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        move.z = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        Vector3 forward = transform.forward;
        forward.y = 0;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, forward);
        move = rotation * move;

        transform.position += move;
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButton(2))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity * 10;
            transform.Rotate(new Vector3(0, mouseX, 0));
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void GetHeight()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        heightLerp -= scroll;
        heightLerp = Mathf.Clamp01(heightLerp);
        float targetHeight = Mathf.Lerp(minHeight, maxHeight, heightLerp);
        smoothedHeight = Mathf.Lerp(smoothedHeight, targetHeight, Time.deltaTime * scrollSmoothSpeed);
        cam.transform.localPosition = new Vector3(0, smoothedHeight, -smoothedHeight);
    }
}
