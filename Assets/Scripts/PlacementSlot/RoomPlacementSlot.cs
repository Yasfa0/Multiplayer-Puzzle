using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlacementSlot : PlacementSlot
{
    [SerializeField] RoomType targetRoomType;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RoomClass>() && (int)other.GetComponent<RoomClass>().GetRoomType() == (int)targetRoomType)
        //if (other.GetComponent<RoomClass>())
        {
            Debug.Log("Other Room Type : " +  (int)other.GetComponent<RoomClass>().GetRoomType());
            Debug.Log("Placement Room Type : " + (int)targetRoomType);
            isOccupied = true;
            Debug.Log("Slot is occupied");
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<RoomClass>() && (int)other.GetComponent<RoomClass>().GetRoomType() == (int)targetRoomType)
        //if (other.GetComponent<RoomClass>())
        {
            Debug.Log("Other Room type : " + (int)other.GetComponent<RoomClass>().GetRoomType());
            Debug.Log("Other Room type : " + (int)targetRoomType);
            isOccupied = false;
            Debug.Log("Slot is unoccupied");
        }
    }
}
