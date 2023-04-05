using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Movement Variables
    [SerializeField] private KeyCode sprintKey;
    private CharacterController characterController;
    [SerializeField] private float playerMovementSpeed = 5f;
    [SerializeField] private float playerSprintSpeed = 8f;
    private Vector3 playerVelocity;

    // Throwable Variables
    [SerializeField] private KeyCode interactWithThrowable;
    private bool holdingThrowable = false;
    [SerializeField] private float pickUpRadius = 5f;
    [SerializeField] private LayerMask throwableObjectLayer;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }


    private void Update()
    {

        // PLAYER MOVEMENT OPTIONS ==========


        // Shift to Run =====
        if (Input.GetKeyDown(sprintKey))
        {
            playerMovementSpeed = playerSprintSpeed;
        }
        if(Input.GetKeyUp(sprintKey))
        {
            playerMovementSpeed = 5f;
        }
        if (holdingThrowable == true)
        {
            playerMovementSpeed = 3f;
        }

        // Arrows / WASD to move around =====
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(move * Time.deltaTime * playerMovementSpeed);

        if(move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        characterController.Move(playerVelocity * Time.deltaTime);


        // PLAYER INTERACTING WITH THROWABLE Object =====

        if (Input.GetKeyDown (interactWithThrowable))
        {
            InteractWithThrowable();
        }

    }

    private void ThrowObject()
    {
        // game object will need it's own
        // rigidbody
    }

    private void InteractWithThrowable()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, pickUpRadius, throwableObjectLayer);

        foreach (Collider collider in colliders)
        {
            if(gameObject.layer == throwableObjectLayer)
            {
                Debug.Log("Can Pick Up");
            }
        }


        // if they do - then throw that object

        // if they do not - check if they are close enough to pick up a throwable object

        // if they are not - do nothing

        // if they are - pick that object up
    }
}
