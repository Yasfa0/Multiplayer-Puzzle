using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static UnityEngine.GraphicsBuffer;

public class FloatingChatMsg : NetworkBehaviour
{
    Transform followTarget;
    [SerializeField] float zOffset;

    private void Update()
    {
        if (followTarget)
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(followTarget.transform.position.x, transform.position.y, followTarget.transform.position.z - zOffset), 1);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        DelayDespawnServerRpc(3f);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DelayDespawnServerRpc(float delay)
    {
        Destroy(gameObject, delay);
    }

    public void SetFollowTarget(Transform followTarget)
    {
        this.followTarget = followTarget;
    }
}
