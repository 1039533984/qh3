using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerController : NetworkBehaviour
{
    Vector3 serverPlayerPosition;
    public override void OnNetworkSpawn()
    {
        gameObject.name += OwnerClientId;
        Move();
    }

    public void Move()
    {
        var pos = GetRandomPositionOnPlane();
        SubmitPositionRequestServerRpc(pos);
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(Vector3 pos, ServerRpcParams rpcParams = default)
    {
        SubmitPositionRequestClientRpc(pos);
    }

    [ClientRpc]
    void SubmitPositionRequestClientRpc(Vector3 newPos, ClientRpcParams rpcParams = default)
    {
        if (IsOwner)
        {
            return;
        }
        transform.position = newPos;
    }

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, UnityEngine.Random.Range(-3f, 3f));
    }
}

