using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; //Locking the Mouse
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation += mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -50f, 50f); //Clamping

        playerBody.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
