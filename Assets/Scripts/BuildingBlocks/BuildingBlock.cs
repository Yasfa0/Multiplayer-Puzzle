using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock : MonoBehaviour
{
    protected bool boxAdded = false;


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            Debug.Log("Block collide with Ground");
            CraneManager.instance.PlaceBlock();
            AddDeleteBox();
        }
    }

    protected void AddDeleteBox()
    {
        if(!boxAdded)
        {
            StageManager.Instance.AddDeleteBox(gameObject);
            boxAdded = true;
        }
    }
}
