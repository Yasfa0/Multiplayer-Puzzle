using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSlot : MonoBehaviour
{
    protected Rigidbody rb;
    [SerializeField] protected string targetTag = "Item";
    [SerializeField] protected float score = 50;
    protected bool isOccupied = false;
    protected bool meshVisibility = true;
    protected MeshRenderer meshRenderer;

    [SerializeField] private FurnitureType furnitureType;

    protected void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();  
        rb = GetComponent<Rigidbody>();
    }

    virtual protected void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<FurnitureClass>() && other.GetComponent<FurnitureClass>().GetFurnitureType() == furnitureType)
        {
            isOccupied = true;
            Debug.Log("Slot is occupied");
        }
    }

    virtual protected void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FurnitureClass>() && other.GetComponent<FurnitureClass>().GetFurnitureType() == furnitureType)
        {
            isOccupied = false;
            Debug.Log("Slot is unoccupied");
        }
    }

    public bool GetOccupied()
    {
        return isOccupied;
    }

    public float GetScore()
    {
        return score;
    }

    public void ToggleMesh()
    {
        meshVisibility = !meshVisibility;
        meshRenderer.enabled = meshVisibility;
    }
}
