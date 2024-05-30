using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseMovement : MonoBehaviour
{

    public Transform orientation;

    float xRotation;
    float yRotation;
    public float sensMultiplier;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sensMultiplier = PlayerPrefs.GetFloat("masterSens");

        if (sensMultiplier >= 0)
        {
            sensMultiplier = 2.0f;
        }

        int invert = PlayerPrefs.GetInt("masterInvertMouse");
        sensMultiplier = sensMultiplier / 2;

        if (invert == 1)
        {
            sensMultiplier = sensMultiplier * -1;
        }
    }

    private void Update()
    {
        if (!PauseManager.isPaused)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * GlobalReferences.Instance.sensX * sensMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * GlobalReferences.Instance.sensY * sensMultiplier;

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -85f, 85f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }
}
