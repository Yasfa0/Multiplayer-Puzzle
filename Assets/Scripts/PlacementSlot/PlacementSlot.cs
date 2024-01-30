using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSlot : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float score = 50;
    private bool isOccupied = false;
    private bool meshVisibility = true;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();  
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Item")
        {
            isOccupied = true;
            Debug.Log("Slot is occupied");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Item")
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
