using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public GameObject TestPrefab;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();

            SubmitNewPosition();
        }

        GUILayout.EndArea();
    }

    void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
        GUILayout.Label("Mode: " + mode);
    }

    void SubmitNewPosition()
    {
        if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
        {
            if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
            {
                foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerController>().Move();
            }
            else
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<PlayerController>();
                player.Move();
            }
        }
        if (NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Spawn" ))
            {
                var obj = Instantiate(TestPrefab, GetRandomPositionOnPlane(), Quaternion.identity);
                obj.GetOrAddComponent<NetworkItem>().NetworkObject.Spawn(true);
            }
        }
    }

    Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, UnityEngine.Random.Range(-3f, 3f));
    }
}

