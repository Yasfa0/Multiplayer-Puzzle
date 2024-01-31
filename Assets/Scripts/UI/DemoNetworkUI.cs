using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class DemoNetworkUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() => { NetworkManager.Singleton.StartServer(); Debug.Log("Server");  } );
        hostBtn.onClick.AddListener(() => { NetworkManager.Singleton.StartHost(); Debug.Log("Server"); });
        clientBtn.onClick.AddListener(() => { NetworkManager.Singleton.StartClient(); Debug.Log("Server"); });
    }

}
