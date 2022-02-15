using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float walkingSpeed = 10f;
    public float sprintingSpeed = 14f;
    Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight;

    public bool isGrounded;
    public Transform playerBody;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    void Start()
    {
        
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.Log(isGrounded);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal"); //A and D
        float z = Input.GetAxis("Vertical"); //W and S

        Vector3 move = -playerBody.right * x - playerBody.forward * z;
        move.y = 0;
        move.Normalize();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            controller.Move(move * sprintingSpeed * Time.deltaTime);
        }
            else controller.Move(move * walkingSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime; 

        controller.Move(velocity * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            Debug.Log(velocity.y);
        }

    }
}
