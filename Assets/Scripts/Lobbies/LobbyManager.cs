using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject joinedPanel;

    [SerializeField] TextMeshProUGUI roomNameTM;
    [SerializeField] GameObject playerInfoPrefab;
    [SerializeField] GameObject playerBoxGrid;

    [SerializeField] TextMeshProUGUI playerTM;
    [SerializeField] GameObject roomBoxPrefab;
    [SerializeField] GameObject roomBoxGrid;

    [SerializeField] Button createBtn;
    [SerializeField] Button refreshBtn;

    List<GameObject> roomBoxes = new List<GameObject>();
    List<GameObject> playerBoxes = new List<GameObject>();

    private bool insideARoom = false;

    private Lobby joinedLobby;
    private Lobby hostLobby;
    private float lobbyHeartbeat = 15;
    private float lobbyPollTimer = 1.1f;
    private string playerName;


    public static LobbyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        createBtn.onClick.AddListener(() => { CreateRoom(); });
        refreshBtn.onClick.AddListener(() => { ListRooms(); }) ;

        joinedPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        insideARoom = false;
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Current Player ID: " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerName = "Slave " + Random.Range(10, 99);
        playerTM.SetText(playerName);
        Debug.Log("Your player name: " + playerName);
    }

    private void Update()
    {
        LobbyUpdatePoll();
        LobbyHeartbeat();
    }

    //Heartbeat & Poll
    private async void LobbyUpdatePoll()
    {

        try
        {
            if (joinedLobby != null)
            {
                lobbyPollTimer -= Time.deltaTime;
                if (lobbyPollTimer < 0)
                {
                    lobbyPollTimer = 1.1f;

                    Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                    joinedLobby = lobby;

                    if (insideARoom)
                    {
                        PrintPlayers(joinedLobby);
                    }
                    if (!insideARoom)
                    {
                        //ListRooms();
                    }

                    if (joinedLobby.Data["RelayCode"].Value != "0")
                    {
                        if (!IsHost)
                        {
                            TestRelay.Instance.JoinRelay(joinedLobby.Data["RelayCode"].Value);
                            gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void LobbyHeartbeat()
    {
        try
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
        catch(LobbyServiceException e) 
        {
            Debug.Log(e);
        }
    }

    //Create Room
    // 1. Create Room.
    // Room Options. Name = Host, Max Player = 4. Stage = Tutorial
    // Run ListRoom()

    private async void CreateRoom()
    {
        try
        {
            CreateLobbyOptions options = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {"Stage", new DataObject(DataObject.VisibilityOptions.Public,"Tutorial Stage") },
                    {"RelayCode", new DataObject(DataObject.VisibilityOptions.Public,"0") }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(playerName, 4, options);
            hostLobby = lobby;
            joinedLobby = hostLobby;
            Debug.Log("Created Lobby: " + lobby.Name + " Max Players: " + lobby.MaxPlayers + " | " + lobby.Id + " | " + lobby.LobbyCode); ;

            //JoinRoom(hostLobby);
            //JoinLobbyByCode(hostLobby.LobbyCode);
            //ListRooms();

            lobbyPanel.SetActive(false);
            joinedPanel.SetActive(true);
            insideARoom = true;

            PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e) { Debug.Log(e); }
    }


    //List Room
    // Clear all room box
    // Search all public room
    // Generate room box based on public room info

    private async void ListRooms()
    {
        try
        {
            if(roomBoxes.Count > 0)
            {
                foreach (GameObject box in roomBoxes)
                {
                    Destroy(box);
                }
                roomBoxes.Clear();
            }

            QueryResponse response = await Lobbies.Instance.QueryLobbiesAsync();

            if (response.Results.Count > 0)
            {
                Debug.Log("Existing lobby: " + response.Results.Count + ". Lucky you!");
                foreach (Lobby lobby in response.Results)
                {
                    GameObject roomBox = GameObject.Instantiate(roomBoxPrefab,roomBoxGrid.transform ,false);
                    //roomBox.transform.SetParent(roomBoxGrid.transform, false);
                    roomBox.GetComponent<RoomInfo>().SetRoomInfo(lobby);
                    roomBox.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(lobby.Id); });
                    roomBoxes.Add(roomBox);

                    Debug.Log("Lobby: " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["Stage"].Value);
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

    //Join Room
    // Masukin method ini ke onClick listener Room Info Box
    // Join ke room by code
    // Matiin lobby panel, nyalain room panel

    public async void JoinRoom(string lobbyId)
    {
        try
        {
            JoinLobbyByIdOptions options = new JoinLobbyByIdOptions
            {
                Player = GetPlayer()
            };

            Lobby curLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId, options);
            //Lobby curLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, options);
            joinedLobby = curLobby;
            Debug.Log("Joined Lobby. Id: " + lobbyId);

            lobbyPanel.SetActive(false);
            joinedPanel.SetActive(true);
            insideARoom = true;

            PrintPlayers(joinedLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //Show Players
    // Jalanin setelah join room atau create room
    // Tampilin room name
    // Clear list playerBoxes
    // Loop tampilin semua player yang ada
    // Taro ke list playerBoxes

    private void PrintPlayers(Lobby lobby)
    {
        //Debug.Log("Player in lobby " + lobby.Name + " " + lobby.Data["GameMode"].Value + " " + lobby.Data["Map"].Value);
        roomNameTM.SetText(lobby.Name);

        if(playerBoxes.Count > 0)
        {
            foreach (GameObject box in playerBoxes)
            {
                Destroy(box);
            }
            playerBoxes.Clear();
        }

        foreach (Player player in lobby.Players)
        {
            //Debug.Log("Player: " + player.Id + " " + player.Data["PlayerName"].Value);
            //Debug.Log("Player: " + player.Id);
            GameObject playerInfo = Instantiate(playerInfoPrefab,playerBoxGrid.transform,false);
            playerInfo.GetComponent<PlayerInfo>().SetPlayerInfo(player);
            playerBoxes.Add(playerInfo);
        }
    }


    //Start Game

    public async void StartGame()
    {
        try
        {
            string relayCode = await TestRelay.Instance.CreateRelay();

            UpdateLobbyOptions options = new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {"RelayCode", new DataObject(DataObject.VisibilityOptions.Public, relayCode) }
                }
            };

            Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, options);

            joinedLobby = lobby;
            gameObject.SetActive(false);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //Leave Room
    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            lobbyPanel.SetActive(true);
            joinedPanel.SetActive(false);
            insideARoom = false;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //Getter & Setter
    public Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject> {
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public,playerName) } }
        };
    }
}
