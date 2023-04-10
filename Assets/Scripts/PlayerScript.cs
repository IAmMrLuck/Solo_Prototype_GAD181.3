using ConaLuk.TopDown;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Conaluk.TopDown
{

    public class PlayerScript : MonoBehaviour
    {
        [Header("Combat Stats")]
        //==========================================================
        [SerializeField] private int playerHitCounter;
        [SerializeField] private int playerHitLimit;
        private bool playerHitLimitReached;



        [Header("Movement Variables")]
        //==========================================================
        [SerializeField] private KeyCode sprintKey;
        private CharacterController characterController;
        [SerializeField] private float playerMovementSpeed = 5f;
        [SerializeField] private float playerSprintSpeed = 8f;
        [SerializeField] private float playerGravity = -10f;
        private Vector3 playerVelocity;
        private bool groundedPlayer;


        [Header("Interaction wtih Throwables")]
        //==========================================================
        [SerializeField] private GameObject playerGameObject;
        [SerializeField] private GameObject throwableGameObject;
        [SerializeField] private KeyCode interactWithThrowable;
        private bool holdingThrowable = false;
        [SerializeField] private float raycastDistance = 1f;
        [SerializeField] private LayerMask throwableObjectLayer;
        [SerializeField] private Rigidbody throwableObjectRB;
        [SerializeField] private float throwableObjectSpeed;
        public ThrowableObjectScript throwHeldObject;



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
            if (Input.GetKeyUp(sprintKey))
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

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
            characterController.Move(playerVelocity * Time.deltaTime);
            playerVelocity.y += playerGravity * Time.deltaTime;


            // PLAYER INTERACTING WITH THROWABLE Object =====
            if (holdingThrowable == true && Input.GetKeyDown(interactWithThrowable))
            {
                ThrowableObjectScript.ThrowHeldObject();
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

            if (playerHitCounter >= playerHitLimit)
            {
                playerHitLimitReached = true;
            }
            if (playerHitLimitReached == true)
            {
                EndGame();
            }

        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                PlayerTakeDamage();
            }
        }

        // this function will only run if there is another throwable object in the raycast'
        // I need to move that 


        private void InteractWithThrowable()
        {
            throwableGameObject.GetComponent<Rigidbody>().isKinematic = true;
            throwableGameObject.transform.position = playerGameObject.transform.position;
            throwableGameObject.transform.parent = playerGameObject.transform;
            holdingThrowable = true;

        }

        void EndGame()
        {
            Destroy(playerGameObject);
            SceneManager.LoadScene("Game Over");
        }

        void PlayerTakeDamage()
        {
            playerHitCounter++;
        }
    }
}