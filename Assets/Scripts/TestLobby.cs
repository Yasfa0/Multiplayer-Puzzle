using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;

public class TestLobby : MonoBehaviour
{
    private Lobby joinedLobby;
    private Lobby hostLobby;
    private float lobbyHeartbeat = 15;
    private float lobbyPollTimer = 1.1f;
    private string playerName;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Current Player ID: " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerName = "Duo " + Random.Range(10, 99);
        Debug.Log("Your player name: " + playerName);
    }

    private async void LobbyUpdatePoll()
    {
        if (joinedLobby != null)
        {
            lobbyPollTimer -= Time.deltaTime;
            if (lobbyPollTimer < 0)
            {
                lobbyPollTimer = 1.1f;

               Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
               joinedLobby = lobby;
            }
        }
    }

    private async void LobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            lobbyHeartbeat -= Time.deltaTime;
            if (lobbyHeartbeat < 0)
            {
                lobbyHeartbeat = 15;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    //CRUD
    // CREATE ==============

    private async void CreateLobby()
    {
        try
        {
            CreateLobbyOptions options = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public,"Build Everything")},
                    {"Map", new DataObject(DataObject.VisibilityOptions.Public,"TutorialMap") }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync("PuzzleLobby1", 4, options);
            hostLobby = lobby;
            joinedLobby = hostLobby;
            Debug.Log("Created Lobby: " + lobby.Name + " Max Players: " + lobby.MaxPlayers + " | " + lobby.Id + " | " + lobby.LobbyCode); ;
            PrintPlayers(hostLobby);
            //JoinLobbyByCode(hostLobby.LobbyCode);
        }
        catch (LobbyServiceException e) { Debug.Log(e); }
    }


    // READ ==============

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Player in lobby " + lobby.Name + " " + lobby.Data["GameMode"].Value + " " + lobby.Data["Map"].Value);

        foreach (Player player in lobby.Players)
        {
            Debug.Log("Player: " + player.Id + " " + player.Data["PlayerName"].Value);
            //Debug.Log("Player: " + player.Id);
        }
    }

    private async void ListLobby()
    {
        try
        {
            /*QueryLobbiesOptions option = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder>
            {
                new QueryOrder(false,QueryOrder.FieldOptions.Created)
            }
            };*/


            QueryResponse response = await Lobbies.Instance.QueryLobbiesAsync();

            if (response.Results.Count > 0)
            {
                Debug.Log("Existing lobby: " + response.Results.Count + ". Lucky you!");
                foreach (Lobby lobby in response.Results)
                {
                    Debug.Log("Lobby: " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);
                }
            }
            else
            {
                Debug.Log("There are no lobbies. Go play a better game.");
            }

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject> {
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public,playerName) } }
        };
    }

    // UPDATE ========

    private async void JoinLobby()
    {
        try
        {
            JoinLobbyByIdOptions options = new JoinLobbyByIdOptions
            {
                Player = GetPlayer()
            };

            QueryResponse response = await Lobbies.Instance.QueryLobbiesAsync();

            Lobby curLobby = await Lobbies.Instance.JoinLobbyByIdAsync(response.Results[0].Id, options);
            joinedLobby = curLobby;
            Debug.Log("Joined Lobby. Id: " + response.Results[0].Id);
            PrintPlayers(curLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            Lobby jLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, options);
            joinedLobby = jLobby;
            PrintPlayers(jLobby);

            Debug.Log("Joined lobby with code " + lobbyCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void QuickJoinLobby()
    {
        try
        {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void UpdateLobbyGameMode(string newMode)
    {
        try
        {
            UpdateLobbyOptions options = new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, newMode) }
                }
            };

            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, options);
            joinedLobby = hostLobby;
            PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void UpdatePlayerName(string playerName)
    {
        try
        {

            UpdatePlayerOptions options = new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
                }
            };

            hostLobby = await Lobbies.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, options);
            joinedLobby = hostLobby;
            PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    // DELETE =======

    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
        Debug.Log(e);
        }
    }

    private async void KickPlayer()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void Update()
    {
        LobbyHeartbeat();
        LobbyUpdatePoll();

        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateLobby();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ListLobby();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            JoinLobby();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuickJoinLobby();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintPlayers(hostLobby);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpdateLobbyGameMode("Free For All");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            UpdatePlayerName("Saul Badguy");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveLobby();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DeleteLobby();
        }
    }
}
