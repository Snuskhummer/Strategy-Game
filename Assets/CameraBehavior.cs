using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] int DragBind;
    [SerializeField] int RotationBind;

    Transform cameraTransform;

    float fastSpeed = 3f;
    float normalSpeed = 0.5f;
    int closestZoomY = 350;

    float movementSpeed;
    float movementTime = 5f;
    float rotationAmount = 0.3f;
    Vector3 zoomAmount = new Vector3(0f, 1f, -1f);
    
    Vector3 newPosition;
    Quaternion newRotation;
    Vector3 newZoom;

    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;
    Vector3 rotateStartPosition;
    Vector3 rotateCurrentPosition;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = transform.GetChild(0).transform;
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
    }

    void InputHandler()
    {
        HandleKeyboardInput();
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        // Zoom
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom -= Input.mouseScrollDelta.y * zoomAmount * 50;
        }

        // Drag
        if (Input.GetMouseButtonDown(DragBind))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }    
        }
        if (Input.GetMouseButton(DragBind))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }

        // Rotation
        if (Input.GetMouseButtonDown(RotationBind))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(RotationBind))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 diff = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * -diff.x / 5f);
        }
    }

    void HandleKeyboardInput()
    {
        // Speed of movement
        switch (Input.GetKey(KeyCode.LeftShift))
        {
            case true:
                movementSpeed = fastSpeed; break;
            case false: movementSpeed = normalSpeed; break;
        }

        // Movement
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += transform.forward * movementSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += transform.right * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += transform.forward * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += transform.right * movementSpeed;
        }

        // Rotation
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        // Zoom
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom += -zoomAmount;
        }

        // Finale
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);

        if (Math.Abs(cameraTransform.localPosition.y) < closestZoomY)
        {
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, closestZoomY, cameraTransform.localPosition.z);
        }
    }
}

    
