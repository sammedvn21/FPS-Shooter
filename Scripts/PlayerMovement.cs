using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;





    [Header("Movement")]
    public float walkSpeed = 12f;
    public float runSpeed = 20f;
    public float gravity = -9.8f;

    [Header("JUMP")]
    public float jumpHeight = 3.0f;
    Vector3 velocity;
    public LayerMask groundMask;
    bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    [Header("Animations")]
    public Animator movementAnimator;
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!Input.anyKey)
        {
            movementAnimator.SetInteger("Movement", 0);
        }



        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {

            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.forward * z;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(move * runSpeed * Time.deltaTime);
                movementAnimator.SetInteger("Movement", 2);


                if ( Input.GetKeyUp(KeyCode.A) ||  Input.GetKeyUp(KeyCode.D))
                {
                    movementAnimator.SetInteger("Movement", 2);
                }

                if (Input.GetKey(KeyCode.S) && Input.GetKeyUp(KeyCode.A) || Input.GetKey(KeyCode.S) && Input.GetKeyUp(KeyCode.D))
                {
                    movementAnimator.SetInteger("Movement", 2);
                }


            }
            else
            {
                controller.Move(move * walkSpeed * Time.deltaTime);
                movementAnimator.SetInteger("Movement", 1);
            }



        }
       /* if(!Input.anyKey || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) 
             || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            movementAnimator.SetInteger("Movement", 0);
        }*/
      



        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            float x = Input.GetAxis("Horizontal");
            Vector3 move = transform.right * x;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(move * runSpeed * Time.deltaTime);
                movementAnimator.SetInteger("Movement", 2);
            }
            else
            {
                controller.Move(move * walkSpeed * Time.deltaTime);
                movementAnimator.SetInteger("Movement", 1);
            }
        }
      







        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }
}
