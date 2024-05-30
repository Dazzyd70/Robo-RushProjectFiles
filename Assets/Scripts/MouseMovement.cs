using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{ 
    public float mouseSensitivity = 2000f;

    float xRotation = 0;
    float yRotation = 0;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    // Start is called before the first frame update
    void Start()
    {
        // Lock cursor and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Get inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate around x
        xRotation -= mouseY;

        // Clamp rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //Rotate around y
        yRotation += mouseX;

        // Apply to transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
