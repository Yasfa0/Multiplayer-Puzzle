using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using System.Globalization;
using UnityEngine.UI;
using System.Threading.Tasks;

public class TestRelay : NetworkBehaviour
{
    public static TestRelay Instance { get; private set; }

    private string playerName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        /* await UnityServices.InitializeAsync();

         AuthenticationService.Instance.SignedIn += () => {
             Debug.Log("Current Player ID: " + AuthenticationService.Instance.PlayerId);
         };

         await AuthenticationService.Instance.SignInAnonymouslyAsync();
         playerName = "Duo " + Random.Range(10, 99);
         Debug.Log("Your player name: " + playerName);*/
    }

    public async Task<string> CreateRelay()
    {
        try
        {
           Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Relay Code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation,"dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            return joinCode;
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);

            return null;
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation,"dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void Update()
    {
        /*if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CreateRelay();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                JoinRelay();
            }
        }*/
    }

}
