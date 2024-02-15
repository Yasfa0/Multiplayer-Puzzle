using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneGridPos : MonoBehaviour
{
    Camera screenCamera;
    Vector3 lastPos;

    [SerializeField] GameObject crane;
    [SerializeField] LayerMask groundLayerMask;

    private void Awake()
    {
        screenCamera = Camera.main;
    }

    public Vector3 GetSelectedMapPosition()
    {
        //Vector3 cranePos = crane.transform.position;
        //cranePos.z = screenCamera.nearClipPlane;
        //Ray ray = screenCamera.ScreenPointToRay(cranePos);
        
        RaycastHit hit;

        if (Physics.Raycast(crane.transform.position, transform.TransformDirection(-Vector3.up),out hit, Mathf.Infinity, groundLayerMask))
        {
            lastPos = hit.point;
        }

        //Debug.Log("Last Pos: " + lastPos);
        return lastPos;
    }
}
