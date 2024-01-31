using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerChatMsg : NetworkBehaviour
{
    //Unoptimized. Change to list or network object reference later
    [SerializeField] GameObject msgOne;
    [SerializeField] GameObject msgTwo;

    GameObject currentMsg = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (IsOwner) { SpawnMsgOneServerRpc(); }
        }else if (Input.GetKeyDown(KeyCode.G))
        {
            if (IsOwner) { SpawnMsgTwoServerRpc(); }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnMsgOneServerRpc()
    {
        currentMsg = Instantiate(msgOne);
        currentMsg.GetComponent<NetworkObject>().Spawn(true);
        currentMsg.transform.position = new Vector3(0, currentMsg.transform.position.y, -20);
        currentMsg.GetComponent<FloatingChatMsg>().SetFollowTarget(transform);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnMsgTwoServerRpc()
    {
        currentMsg = Instantiate(msgTwo);
        currentMsg.GetComponent<NetworkObject>().Spawn(true);
        currentMsg.transform.position = new Vector3(0, currentMsg.transform.position.y, -20);
        currentMsg.GetComponent<FloatingChatMsg>().SetFollowTarget(transform);
    }
}
