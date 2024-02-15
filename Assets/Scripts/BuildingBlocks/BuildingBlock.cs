using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            Debug.Log("Block collide with Ground");
            CraneManager.instance.PlaceBlock();
        }
    }
}
