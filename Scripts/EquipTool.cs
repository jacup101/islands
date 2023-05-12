using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipTool : MonoBehaviour
{
    [field: SerializeField] public Tool Tool {get; set;}
    private Harvesting _targetHarvesting;

    private void Start()
    {
        // NEED TO FIND WORK AROUND FOR MULTIPLAYER GAME
        // There are two solutions to this
        // Option 1 - move this into a function called by player network
        // Option 2 - add a corresponding EquipToolNetwork behavior
        // I think that player network is cleaner if we don't do that a whole lot, 
        // otherwise splitting it is cleaner to avoid clutter
        // _targetHarvesting = FindObjectOfType<PlayerController>().GetComponentInChildren<Harvesting>();
    }

    public void NetStart(Harvesting h) {
        _targetHarvesting = h;
    }

    public void ChangeTool()
    {
        if(_targetHarvesting != null)
        {
            _targetHarvesting.EquippedTool = Tool;
        }
        else
        {
            Debug.LogWarning("Target harvesting component no longer referenced. Is the player still active  in the screen?");
        }
    }

    public void SetTool(Tool new_tool, Sprite img) {
        bool isHolding = false;
        if (_targetHarvesting.EquippedTool == Tool) {
            isHolding = true;
        }
        // Set the tool
        this.Tool = new_tool;
        // Set the image
        this.gameObject.GetComponent<Image>().sprite = img;
        if (isHolding) {
            ChangeTool();
        }
    }

    public string GetToolType() {
        return Tool.DisplayName;
    }
}
