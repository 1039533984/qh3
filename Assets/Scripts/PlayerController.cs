﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerController : NetworkBehaviour
{

    public override void OnNetworkSpawn()
    {
        gameObject.name += OwnerClientId;
    }

    public void Move()
    {
        SubmitPositionRequestServerRpc();
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        var randomPosition = GetRandomPositionOnPlane();
        //transform.position = randomPosition;
        SubmitPositionRequestClientRpc(rpcParams.Receive.SenderClientId, randomPosition);
    }

    [ClientRpc]
    void SubmitPositionRequestClientRpc(ulong clientId, Vector3 newPos, ClientRpcParams rpcParams = default)
    {
        NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.transform.position = newPos;
    }

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, UnityEngine.Random.Range(-3f, 3f));
    }
}

