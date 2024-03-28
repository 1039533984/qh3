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

    public override void OnNetworkSpawn()
    {
        gameObject.name += OwnerClientId;
        Move();
    }

    public void Move()
    {
        var pos = GetRandomPositionOnPlane();
        if (!IsServer)
        {
            transform.position = pos;
        }
        //SubmitPositionRequestServerRpc(pos);
    }

    //[ServerRpc]
    //void SubmitPositionRequestServerRpc(Vector3 pos, ServerRpcParams rpcParams = default)
    //{
    //    transform.position = pos;
    //}

    //[ClientRpc]
    //void SubmitPositionRequestClientRpc(Vector3 newPos, ClientRpcParams rpcParams = default)
    //{
        
    //}

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, UnityEngine.Random.Range(-3f, 3f));
    }
}

