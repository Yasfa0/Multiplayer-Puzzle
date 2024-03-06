using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlacementSlot : PlacementSlot
{
    [SerializeField] RoomType targetRoomType;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RoomClass>() && other.GetComponent<RoomClass>().GetRoomType() == targetRoomType)
        {
            isOccupied = true;
            Debug.Log("Slot is occupied");
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<RoomClass>() && other.GetComponent<RoomClass>().GetRoomType() == targetRoomType)
        {
            isOccupied = false;
            Debug.Log("Slot is unoccupied");
        }
    }
}
