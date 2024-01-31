using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerGrabArea : NetworkBehaviour
{
    private GameObject inGrabArea = null;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Item")
        {
            inGrabArea = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Item")
        {
            inGrabArea = null;
        }
    }

    public GameObject GetInGrabArea()
    {
        return inGrabArea;
    }
}
