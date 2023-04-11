using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Conaluk.TopDown
{
    public class PlateTrigger : MonoBehaviour
    {

        [SerializeField] private WallManager WallManager;

        void OnTriggerEnter(Collider other)
        {
            WallManager.triggerPlate = true;
        }
    }
}