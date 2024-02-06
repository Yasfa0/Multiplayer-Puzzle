using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameTM;

    Player player;

    public void SetPlayerInfo(Player player)
    {
        this.player = player;
        playerNameTM.SetText(this.player.Data["PlayerName"].Value);
    }
}
