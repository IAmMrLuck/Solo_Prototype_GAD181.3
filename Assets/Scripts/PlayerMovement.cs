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
    [SerializeField] private float playerGravity = -10f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    // Throwable Variables =====
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private GameObject throwableGameObject;
    [SerializeField] private KeyCode interactWithThrowable;
    private bool holdingThrowable = false;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] private LayerMask throwableObjectLayer;
    [SerializeField] private Rigidbody throwableObjectRB;
    [SerializeField] private float throwableObjectSpeed;



    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }


    private void Update()
    {

        // PLAYER MOVEMENT OPTIONS ==========

        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

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
        playerVelocity.y += playerGravity *Time.deltaTime;


        // PLAYER INTERACTING WITH THROWABLE Object =====
        if (holdingThrowable == true && Input.GetKeyDown(interactWithThrowable))
        {
            ThrowObject();
        }

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
        throwableObjectRB.AddRelativeForce(0, 0, throwableObjectSpeed);

    }

    private void InteractWithThrowable()
    { 
        throwableGameObject.GetComponent<Rigidbody>().isKinematic = true; 
        throwableGameObject.transform.position = playerGameObject.transform.position;
        throwableGameObject.transform.parent = playerGameObject.transform;
        holdingThrowable = true;

    }
}
