using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Lobbies.Models;

public class RoomInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hostNameTM;
    [SerializeField] TextMeshProUGUI roomCapacityTM;
    [SerializeField] TextMeshProUGUI stageNameTM;

    Lobby lobby;

    public void SetRoomInfo(Lobby lobby)
    {
        this.lobby = lobby;

        hostNameTM.SetText(this.lobby.Name);
        roomCapacityTM.SetText(this.lobby.Players.Count + "/" + this.lobby.MaxPlayers);
        stageNameTM.SetText(this.lobby.Data["Stage"].Value);
    }

}
