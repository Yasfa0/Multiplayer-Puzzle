using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlacedRoomBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    GameObject roomRef;

    public void SetData(GameObject roomRef, string roomName)
    {
        nameText.text = roomName;
        this.roomRef = roomRef;
    }

    public void DeleteRoom()
    {
        Destroy(roomRef);
        Destroy(gameObject);
    }
}
