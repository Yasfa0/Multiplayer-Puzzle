using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisjointedCCollider : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if(gameObject.tag == "Ground")
        {
            Debug.Log("D Child Ground hit");
            gameObject.GetComponentInParent<DisjointedBBlock>().BlockCollideWGround();
        }
    }
}
