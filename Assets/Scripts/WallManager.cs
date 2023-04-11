using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conaluk.TopDown
{

    public class WallManager : MonoBehaviour
    {

        GameObject[] wallsToDissolve;
        public bool triggerPlate = false;


        [SerializeField] private GameObject playerOne;
        [SerializeField] private GameObject playerTwo;
      


        private void Start()
        {
            wallsToDissolve = GameObject.FindGameObjectsWithTag("Dissolvable Wall");
        }

        private void Update()
        {
            if (triggerPlate == true )
            {
                StartCoroutine(DissolveTheWall());
            }
        }



        IEnumerator DissolveTheWall()
        {
            yield return new WaitForSeconds(3f);
            foreach (GameObject wall in wallsToDissolve)
                wall.SetActive(false);
        }

    }
}