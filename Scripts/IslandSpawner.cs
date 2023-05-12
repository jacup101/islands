using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class IslandSpawner : NetworkBehaviour
{
    // Currently Spawns based on network id
    int current_player_id = 0;
    // int max_players = 4;
    // TODO: If we expand map, spawn more dynamically using server and client RPCs

    [field: SerializeField] public Vector3[] spawnPoints;

    public Vector3 GetSpawn(int id) {
        /*
        Vector3 toReturn = spawnPoints[current_player_id];
        AcquireSpawnServerRpc();
        */
        if (id > 3) {
            return spawnPoints[0];
        }
        return spawnPoints[id];
    }

    [ServerRpc(RequireOwnership = false)]
    public void AcquireSpawnServerRpc() {
        current_player_id ++;
        AcquireSpawnClientRpc();
    }

    [ClientRpc]
    public void AcquireSpawnClientRpc() {
        current_player_id ++;
    }

}
