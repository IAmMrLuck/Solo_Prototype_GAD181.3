using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

namespace ConaLuk.TopDown
{
    public class ThrowableObjectScript : MonoBehaviour
    {
        [SerializeField] private GameObject throwableObjectSelf;
        [SerializeField] private float forwardMomentum;
        [SerializeField] private float upwardMomentum;

        [SerializeField] private float throwableObjectGravity;

        public void ThrowHeldObject()
        {
            Debug.Log("LaunchObject Method Called");
            LaunchThrowable();
        }

        private void LaunchThrowable()
        {
            throwableObjectSelf.GetComponent<Rigidbody>().isKinematic = false;
            transform.SetParent(null);
            Vector3 forceToAdd = new Vector3(forwardMomentum, upwardMomentum, 0);
            throwableObjectSelf.GetComponent<Rigidbody>().AddForce(forceToAdd, ForceMode.Impulse);
            ///throwablwObjectRB.AddForce(forceToAdd, ForceMode.Impulse);    
        }
    }
}