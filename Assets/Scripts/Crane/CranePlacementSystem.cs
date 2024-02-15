using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CranePlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject craneIndicator;
    [SerializeField] Grid grid;
    CraneGridPos craneGridPos;
    Vector3 lastIndicatorPos;


    private void Awake()
    {
        craneGridPos = GetComponent<CraneGridPos>();
    }

    private void Update()
    {
        Vector3 cranePos = craneGridPos.GetSelectedMapPosition();
        Vector3Int indicatorPos = grid.WorldToCell(cranePos); 
        //Debug.Log("Crane Pos: " + cranePos);
        //craneIndicator.transform.position = cranePos;
        lastIndicatorPos = grid.CellToWorld(indicatorPos);
        lastIndicatorPos.y = 0;
        craneIndicator.transform.position = lastIndicatorPos;
    }

    public Vector3 GetLastIndicatorPos()
    {
        return lastIndicatorPos;
    }

}
