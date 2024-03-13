using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomClass : MonoBehaviour
{

    [SerializeField] RoomType roomType;

    public RoomType GetRoomType() { return roomType; }

}

public enum RoomType
{
    Basic, Long, Square, TypeOne, TypeTwo, TypeThree
}
