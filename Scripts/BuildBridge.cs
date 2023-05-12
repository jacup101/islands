using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class BuildBridge : NetworkBehaviour
{
    [field: SerializeField] public TileBase highlightTile_horizontal;
    [field: SerializeField] public TileBase highlightTile_vertical;
    private TileBase[] baseArray;
    [field: SerializeField] public Tilemap highlightMap;
    [field: SerializeField] public bool buildMode;
    [field: SerializeField] public Vector2 _facingDir;
    [field: SerializeField] public Inventory _inventory;
    [field: SerializeField] public Resource wood;
    
 
    private Vector3Int previous;
    private TileBase currTile;
    private int currTileInt;
    private Resources currResources;
    private int count;

    public void Start() {
        this.highlightMap = GameObject.Find("Ground").GetComponent<Tilemap>();
        baseArray = new TileBase[]{highlightTile_horizontal, highlightTile_vertical};
    }

    public void SetInventory(Inventory inv) {
        this._inventory = inv;
    }
    
    // do late so that the player has a chance to move in update if necessary
    private void LateUpdate()
    {
        if (buildMode) {

            count = _inventory.GetResourceCount(wood);
            if (count > 1) {
                // get current grid location
                Vector3Int currentCell = highlightMap.WorldToCell(transform.position);
                // add one in a direction (you'll have to change this to match your directional control)
                if (_facingDir.x != 0) {
                    currTile = highlightTile_horizontal;
                    currTileInt = 0;
                    currentCell.x += (int) _facingDir.x;
                } else {
                    currTile = highlightTile_vertical;
                    currTileInt = 1;
                    currentCell.y += (int) _facingDir.y;
                }
                // if the cell is water, good to build bridge
                if(highlightMap.GetTile(currentCell) == null)
                {
                    // set the new tile
                    // highlightMap.SetTile(currentCell, currTile);
                    BuildBridgeServerRpc(currentCell, currTileInt);
        
                    // Remove 2 from Inventory
                    _inventory.AddResources(wood, -2);
                }

                buildMode = false;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void BuildBridgeServerRpc(Vector3Int currentCell, int currTile) {
        BuildBridgeClientRpc(currentCell, currTile);
    }
    [ClientRpc]
    public void BuildBridgeClientRpc(Vector3Int currentCell, int currTile) {
        highlightMap.SetTile(currentCell, baseArray[currTile]);
    }
}
