using ConaLuk.TopDown;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Conaluk.TopDown
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerScript : MonoBehaviour
    {
        [Header("Combat Stats")]
        //==========================================================//
        [SerializeField] private int playerHitCounter;
        [SerializeField] private int playerHitLimit;
        private bool playerHitLimitReached;



        [Header("Movement Variables")]
        //==========================================================//
        [SerializeField] private KeyCode sprintKey;
        private CharacterController characterController;
        [SerializeField] private float playerMovementSpeed = 5f;
        [SerializeField] private float playerSprintSpeed = 8f;
        [SerializeField] private float playerGravity = -10f;
        private Vector3 playerVelocity;
        private bool groundedPlayer;
        private Vector2 playerPosition;
        private bool interacted = false;
        private bool sprinting = false;


        [Header("Interaction wtih Throwables")]
        //==========================================================//
        [SerializeField] private GameObject playerGameObject;
        [SerializeField] private GameObject throwableGameObject;
        [SerializeField] private KeyCode interactWithThrowable;
        private bool holdingThrowable = false;
        [SerializeField] private float raycastDistance = 1f;
        [SerializeField] private LayerMask throwableObjectLayer;
        [SerializeField] private Rigidbody throwableObjectRB;
        [SerializeField] private float throwableObjectSpeed;
        public ThrowableObjectScript throwHeldObject;
        private Vector3 throwableVeloctiy;
        [SerializeField] private float forwardMomentum;
        [SerializeField] private float upwardMomentum;
        [SerializeField] private float downwardMomentum;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            playerPosition = context.ReadValue<Vector2>();

        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            interacted = context.ReadValue<bool>();
            interacted = context.action.triggered;
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            sprinting = context.ReadValue<bool>();
            sprinting = context.action.triggered;
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
            if (sprinting)
            {
                playerMovementSpeed = playerSprintSpeed;
            }
            if (sprinting)
            {
                playerMovementSpeed = 5f;
            }
            if (holdingThrowable == true)
            {                               // can sprint instead? //
                playerMovementSpeed = 3f;
            }

            // Arrows / WASD to move around =====
            Vector3 move = new Vector3(playerPosition.x, 0, playerPosition.y);
            characterController.Move(move * Time.deltaTime * playerMovementSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
            characterController.Move(playerVelocity * Time.deltaTime);
            playerVelocity.y += playerGravity * Time.deltaTime;


            // PLAYER INTERACTING WITH THROWABLE Object =====
            if (holdingThrowable == true && interacted)
            {
                throwableGameObject.transform.SetParent(null);
                Debug.Log("LaunchThrowable Called");
                throwableObjectRB.isKinematic = false;
                throwableObjectRB.AddRelativeForce(transform.forward);
                throwableVeloctiy.y += downwardMomentum * Time.deltaTime;
            }

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastDistance, throwableObjectLayer))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
                Debug.Log("Did Hit");

                if (interacted)
                {
                    InteractWithThrowable();
                    Debug.Log("holding Throwable is True");

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

        private void InteractWithThrowable()
        {
            holdingThrowable = true;
            throwableGameObject.GetComponent<Rigidbody>().isKinematic = true;
            throwableGameObject.transform.position = playerGameObject.transform.position;
            throwableGameObject.transform.parent = playerGameObject.transform;
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