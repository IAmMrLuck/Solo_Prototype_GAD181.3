using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conaluk.TopDown
{
    public class DissolvableWall : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(true);
        }

        public void DisolveWall()
        {
            gameObject.SetActive(false);
        }

    }
}