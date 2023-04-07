using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Movement Variables =====
    [SerializeField] private KeyCode sprintKey;
    private CharacterController characterController;
    [SerializeField] private float playerMovementSpeed = 5f;
    [SerializeField] private float playerSprintSpeed = 8f;
    private Vector3 playerVelocity;

    // Throwable Variables =====
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private GameObject throwableGameObject;
    [SerializeField] private KeyCode interactWithThrowable;
    private bool holdingThrowable = false;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] private LayerMask throwableObjectLayer;
    [SerializeField] private Rigidbody throwableObjectRB;



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

  

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastDistance, throwableObjectLayer))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            Debug.Log("Did Hit");

            if (Input.GetKeyDown(interactWithThrowable))
            {
                InteractWithThrowable();
            }
        }

    }
    // this function will only run if there is another throwable object in the raycast'
    // I need to move that 
    private void ThrowObject()
    {
        throwableGameObject.GetComponent<Rigidbody>().isKinematic = false;
        throwableGameObject.transform.parent = null;
        throwableObjectRB.AddForce(0, 0, 1);
    }

    private void InteractWithThrowable()
    {
        
        if(holdingThrowable == true)
        {
            ThrowObject();
        }
       
        else
        {
            throwableGameObject.GetComponent<Rigidbody>().isKinematic = true; 
            throwableGameObject.transform.position = playerGameObject.transform.position;
            throwableGameObject.transform.parent = playerGameObject.transform;
            holdingThrowable = true;
        }

        // if they do - then throw that object

        // if they do not - check if they are close enough to pick up a throwable object

        // if they are not - do nothing

        // if they are - pick that object up
    }
}
