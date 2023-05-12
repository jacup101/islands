using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour
{
    Vector3 moveDir = new Vector3(0, 0, 0);
    PlayerController player;
    Harvesting harvesting;
    IslandSpawner spawner;
    GameBehaviour gb;
    GameObject loading_screen;
    GameObject dead_screen;
    bool dead = false;
    [SerializeField]
    GameObject health_bar;
    [SerializeField]
    Slider ui_slider;

    [SerializeField] Tool[] tools;

    // "Idle" 0
    // "Walk" 1
    // "Act" 2
    private NetworkVariable<int> netState = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // "Down" - 0
    // "Up" - 1
    // "Left" - 2
    // "Right" - 3
    private NetworkVariable<int> netDirectionState = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    // "Copper Weapon" - 0
    // "Copper Axe" - 1
    // "Copper Pickaxe" - 2
    // "Bridge Hammer" - 3
    // "Iron Weapon" - 4
    // "Iron Axe" - 5
    // "Iron Pickaxe" - 6
    // "Gold Weapon" - 7
    // "Gold Axe" - 8
    // "Gold Pickaxe" - 9
    private NetworkVariable<int> netTool = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<int> health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // Storing vars
    // create structs for inventory and toolbar
    // create a mapping for tools in toolbar (i.e. 1 is a golden hammer, etc.)
    public override void OnNetworkSpawn()
    {
        player = this.GetComponent<PlayerController>();
        gb = GameObject.Find("ServerManager").GetComponent<GameBehaviour>();
        loading_screen = gb.loading;
        dead_screen = gb.dead;
        DetermineSpawn();

        netDirectionState.OnValueChanged += (int prevValue, int newValue) => {
            int[] pair = FakeInput(newValue);
            player.PropagateNetworkDirection(pair[0], pair[1]);
        };

        netState.OnValueChanged += (int prevValue, int newValue) => {
            // Debug.Log(OwnerClientId + "; " + newValue);

            int[] pair = FakeInput(netDirectionState.Value);
            player.PropagateNetworkAnim(newValue, pair[0], pair[1]);
            /*if (IsOwner) {
                netDirectionState.Value = StripDirection(player.PropagateNetworkAnim(newValue, netXIn.Value, netYIn.Value));
            */
        };

        netTool.OnValueChanged += (int prevValue, int newValue) => {
            if(newValue != -1) {
                harvesting.ChangeTool(tools[newValue]);
            }
        };

        if (IsOwner) {
            OwnedResponsibilities();
        } else {
            UnownedResponsibilities();
        }
        
        base.OnNetworkSpawn();


    }
    
    public void Update()
    {
        if(!IsOwner) {
            return;
        }
        player.NetUpdate();
        
        int currentDir = player.GetNetworkDir();
        if(!currentDir.Equals(netDirectionState.Value)) {
            netDirectionState.Value = currentDir;
        }

        int currentAnim = player.GetNetworkAnim();
        if(!currentAnim.Equals(netState.Value)) {
            netState.Value = currentAnim;
        }

        int currentTool = harvesting.GetToolType();
        if(!currentTool.Equals(netTool.Value)) {
            netTool.Value = currentTool;
        }
        
    }

    public void FixedUpdate() {
        if(!IsOwner) {
            return;
        }
        player.NetFixedUpdate();
    }

    private void DetermineSpawn() {
        spawner = GameObject.Find("ServerManager").GetComponent<IslandSpawner>();
        Vector3 pos = spawner.GetSpawn((int) OwnerClientId);
        this.player.transform.position = pos;
    }

    // Owned Responsibilities Manages everything that needs to happen only on client side (i.e. only when the player themselves spawns in)
    private void OwnedResponsibilities() {
        EquipToolSetup();
        SetupResourceCounts();
        FixCamera();
        SetBridgeInventory();
        SetupEnergySliders();
        SetupButtons();
        player.is_client = true;
        health_bar.SetActive(false);
        NewPlayerNotifyServerRpc(OwnerClientId);
    }

    private void UnownedResponsibilities() {
        harvesting = this.gameObject.transform.GetChild(1).GetComponent<Harvesting>();
        RemoveToolCollider(harvesting);
        // this.gameObject.GetComponent<Inventory>().SetCounts(new ResourceCounts());
    }

    [ServerRpc]
    public void NewPlayerNotifyServerRpc(ulong id) {
        gb.AddPlayer(id, this);
    }

    [ServerRpc]
    public void DeadPlayerNotifyServerRpc(ulong id) {
        gb.KillPlayer(id);
    }

    private void SetupButtons() {
        Button b1 = GameObject.Find("Weapon_Tooltip").GetComponent<Button>();
        Button b2 = GameObject.Find("Axe_Tooltip").GetComponent<Button>();
        Button b3 = GameObject.Find("Pickaxe_Tooltip").GetComponent<Button>();
        Button b4 = GameObject.Find("Bridge_Tooltip").GetComponent<Button>();
        player.SetButtons(b1, b2, b3, b4);
    }

    private void RemoveToolCollider(Harvesting h) {
        h.notClient = true;
    }

    private void SetupEnergySliders() {
        Stamina stam = GetComponent<Stamina>();
        stam.SetSlider(GameObject.Find("SB_Fill"));
        stam.SetStamina(GameObject.Find("Stamina_Bar"));
        player.SetHealth(GameObject.Find("HB_Fill"));

    }

    private void EquipToolSetup() {

        EquipTool weapon = GameObject.Find("Weapon_Tooltip").GetComponent<EquipTool>();
        EquipTool axe = GameObject.Find("Axe_Tooltip").GetComponent<EquipTool>();
        EquipTool pickaxe = GameObject.Find("Pickaxe_Tooltip").GetComponent<EquipTool>();
        EquipTool bridge = GameObject.Find("Bridge_Tooltip").GetComponent<EquipTool>();

        harvesting = this.gameObject.transform.GetChild(1).GetComponent<Harvesting>();

        weapon.NetStart(harvesting);
        axe.NetStart(harvesting);
        pickaxe.NetStart(harvesting);
        bridge.NetStart(harvesting);


    }

    private void SetupResourceCounts() {
        Inventory inv = this.GetComponent<Inventory>();
        ResourceCounts counts = GameObject.Find("Resources").GetComponent<ResourceCounts>();
        InventoryMenu invMenu = GameObject.Find("InventoryWrapper").GetComponent<InventoryMenu>();
        inv.SetCounts(counts);
        invMenu.SetInventory(inv);
        GameObject invMenu2 = GameObject.Find("InventoryWrapper").transform.GetChild(0).gameObject;
        player.SetInventoryMenu(invMenu2);
    }

    private void SetupFalseResourceCounts() {

    }

    private void FixCamera() {
        var vcam = FindObjectOfType<CinemachineVirtualCamera>().GetComponent<CinemachineVirtualCamera>();
        vcam.LookAt = this.gameObject.transform;
        vcam.Follow = this.gameObject.transform;
    }

    private void SetBridgeInventory() {
        Inventory inv = this.GetComponent<Inventory>();
        BuildBridge bridge = this.GetComponent<BuildBridge>();
        bridge.SetInventory(inv);
    }


    private int[] FakeInput(int value) {
        // Index 0 - x, 1 - y
        int[] pair = new int[2];
        
        if (value == 0) {
            pair[0] = 0;
            pair[1] = -1;
        }
        if (value == 1) {
            pair[0] = 0;
            pair[1] = 1;
        }
        if (value == 2) {
            pair[0] = -1;
            pair[1] = 0;
        }
        if (value == 3) {
            pair[0] = 1;
            pair[1] = 0;
        }
        return pair;
    }

    public void Damage(int damage) {
        PropagateHealthServerRpc(damage);
    }

    [ServerRpc(RequireOwnership = false)]
    public void PropagateHealthServerRpc(int damage) {
        PropagateHealthClientRpc(damage);
    }

    [ClientRpc]
    public void PropagateHealthClientRpc(int damage) {
        if (IsOwner) {
            health.Value = player.Damage(damage);
            ChangeHealthBarServerRpc(health.Value);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void ChangeHealthBarServerRpc(int health) {
        ChangeHealthBarClientRpc(health);
    }
    [ClientRpc]
    public void ChangeHealthBarClientRpc(int health) {
        if (!IsOwner) {
            ui_slider.value = health;
        }
        if (health <= 0 && !dead) {
            // Show dead UI
            player.can_move = false;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            dead = true;
            if (IsOwner) {
                DeadPlayerNotifyServerRpc(OwnerClientId);
                dead_screen.SetActive(true);
            }
            if (!IsOwner) {
                health_bar.SetActive(false);
            }
        }
    }

    public int GetDamageAmt() {
        // 0 - Copper - 5
        // 4 - Iron - 10
        // 7 - Gold - 25
        if (this.netTool.Value == 0) {
            return 5;
        }
        if (this.netTool.Value == 4) {
            return 10;
        }
        if (this.netTool.Value == 7) {
            return 25;
        }
        return 0;

    }

    public void StartGame() {
        Debug.Log("Trying to start game for " + NetworkBehaviourId);
        // StartGameServerRpc();
        StartGameServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc() {
        Debug.Log("Trying to start game for thru serverrpc for " + NetworkBehaviourId);
        StartGameClientRpc();
    }
    [ClientRpc]
    public void StartGameClientRpc() {
        DebugPrintServerRpc("Running start game client rpc for " + NetworkBehaviourId + " on " + OwnerClientId);
        if (IsOwner) {
            Debug.Log("Starting Game For Player " + OwnerClientId);
            DebugPrintServerRpc("Starting Game For Player " + OwnerClientId);
            player.can_move = true;
            loading_screen.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DebugPrintServerRpc(string toprint) {
        Debug.Log(toprint);
    }

    public void EndGame() {
        EndGameServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    public void EndGameServerRpc() {
        EndGameClientRpc();
    }
    [ClientRpc]
    public void EndGameClientRpc() {
        if (IsOwner) {
            if (health.Value <= 0) {
                // this.dead_screen.SetActive(false);
                gb.dead.SetActive(false);
                gb.loss.SetActive(true);
            } else {
                gb.win.SetActive(true);
            }
        }
    }
    

}