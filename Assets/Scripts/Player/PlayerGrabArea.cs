using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabArea : MonoBehaviour
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
