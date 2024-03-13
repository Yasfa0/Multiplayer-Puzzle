using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisjointedBBlock : BuildingBlock
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Block collide with Ground");
            CraneManager.instance.PlaceBlock();
            AddDeleteBox();
        }
    }

    public void BlockCollideWGround()
    {
        Debug.Log("Block collide with Ground");
        CraneManager.instance.PlaceBlock();
        AddDeleteBox();
    }
}
