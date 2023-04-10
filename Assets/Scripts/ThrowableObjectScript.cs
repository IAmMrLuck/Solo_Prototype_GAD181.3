using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConaLuk.TopDown
{
    public class ThrowableObjectScript : MonoBehaviour
    {
        [SerializeField] private GameObject throwableObjectSelf;
        public float forwardMomentum;
        public float upwardMomentum;
        [SerializeField] private static Rigidbody throwablwObjectRB;
        [SerializeField] private float throwableObjectGravity;

        public void ThrowHeldObject()
        {
            LaunchThrowable();
        }

        public void LaunchThrowable()

        {
            
            Vector3 forceToAdd = new Vector3(forwardMomentum, upwardMomentum, 0);

            throwablwObjectRB.AddForce(forceToAdd, ForceMode.Impulse);

    
        }


    }
}