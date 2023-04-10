using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Conaluk.TopDown
{

    public class EnemyScript : MonoBehaviour
    {
        [Header("Enemy Type")]


        [Header("Enemy Movement")]
        private NavMeshAgent enemyAgent;
        [SerializeField] private GameObject playerGameObject;
        [SerializeField] private float enemyMovementSpeed;

        [Header("Combat Stats")]
        [SerializeField] private GameObject enemyObject;
        [SerializeField] private int enemyHitCounter;
        [SerializeField] private int enemyHitLimit;
        private bool enemyHitLimitReached = false;


        // go and make a flowchart for this guy 
        // and everything else you need

        private void Start()
        {
            enemyHitCounter = 0;
            enemyAgent = GetComponent<NavMeshAgent>();
            playerGameObject = GameObject.FindWithTag("Player");
        }

        private void Update()
        {

            enemyAgent.SetDestination(playerGameObject.transform.position);

            if (enemyHitCounter >= enemyHitLimit)
            {
                enemyHitLimitReached = true;
            }
            if (enemyHitLimitReached == true)
            {
                Destroy(enemyObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Throwable Object"))
            {
                EnemyTakeDamage();
            }
        }

        void EnemyTakeDamage()
        {
            enemyHitCounter++;
        }

    }

}