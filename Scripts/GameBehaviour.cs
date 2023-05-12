using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameBehaviour : NetworkBehaviour
{
    [SerializeField]
    int num_players = 2;
    [SerializeField]
    bool game_started = false;
    [SerializeField]
    List<ulong> players;
    [SerializeField]
    List<ulong> dead_players;
    [SerializeField]
    List<PlayerNetwork> player_networks;
    [SerializeField]
    public GameObject loading;
    public GameObject dead;
    public GameObject win;
    public GameObject loss;
    void Start()
    {
        
    }

    void Update() {
        
    }

    private void StartGame() {
        Debug.Log("Player Network Size: " + player_networks.Count);
        foreach (PlayerNetwork pn in player_networks) {
            pn.StartGame();
        }
    }

    private void EndGame() {
        foreach (PlayerNetwork pn in player_networks) {
            pn.EndGame();
        }
    }

    public void AddPlayer(ulong newplayer, PlayerNetwork pn) {
        if (!players.Contains(newplayer)) {
            players.Add(newplayer);
            player_networks.Add(pn);
            Debug.Log("Player " + newplayer + " connected...");
        } else {
            Debug.LogWarning("Player has already been added");
        }
        if (players.Count == num_players && !game_started ) {
            StartGame();
            game_started = true;
        }
    }

    public void KillPlayer(ulong newplayer) {
        if (!dead_players.Contains(newplayer)) {
            dead_players.Add(newplayer);
        } else {
            Debug.LogWarning("Player has already died");
        }
        
        if (dead_players.Count == num_players - 1) {
            EndGame();
        }
    }
}
