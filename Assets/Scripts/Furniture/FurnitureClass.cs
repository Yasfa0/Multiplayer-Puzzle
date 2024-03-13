using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureClass : MonoBehaviour
{
    [SerializeField] FurnitureType furnitureType;

    public FurnitureType GetFurnitureType()
    {
        return furnitureType;
    }
}

public enum FurnitureType
{
    Box, Long, Kasur, Meja, Kursi, Kulkas, Galon, Microwave
}
