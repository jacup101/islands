using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class InventoryMenu : MonoBehaviour
{
    const int num_upgrade = 10;
    [SerializeField] public EquipTool weapon_type;
    [SerializeField] public EquipTool axe_type;
    [SerializeField] public EquipTool pickaxe_type;
    [SerializeField] public Inventory inv;
    [SerializeField] public List<Resource> resource_types;
    // By Convention
    // 0 - Iron Weapon
    // 1 - Gold Weapon
    // 2 - Iron Axe
    // 3 - Gold Axe
    // 4 - Iron Pickaxe
    // 5 - Gold Pickaxe
    [SerializeField] public List<Tool> tools;
    [SerializeField] public List<Sprite> sprites;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetInventory(Inventory i) {
        this.inv = i;
    }

    public void SetIronText(int amount) {
        if (weapon_type.GetToolType().Contains("Copper")) {
            // Set the text of the button
            this.gameObject.transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "UPGRADE IN " + Mathf.Max(0, num_upgrade - amount) + " IRON";
        }
        if (axe_type.GetToolType().Contains("Copper")) {
            this.gameObject.transform.GetChild(0).GetChild(2).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "UPGRADE IN " + Mathf.Max(0, num_upgrade - amount) + " IRON";

        }
        if (pickaxe_type.GetToolType().Contains("Copper")) {
            this.gameObject.transform.GetChild(0).GetChild(3).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "UPGRADE IN " + Mathf.Max(0, num_upgrade - amount) + " IRON";

        }
        // TODO: do this
    }

    public void SetGoldText(int amount) {
        if (weapon_type.GetToolType().Contains("Iron")) {
            // Set the text of the button
            this.gameObject.transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "UPGRADE IN " + Mathf.Max(0, num_upgrade - amount) + " GOLD";
        }
        if (axe_type.GetToolType().Contains("Iron")) {
            this.gameObject.transform.GetChild(0).GetChild(2).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "UPGRADE IN " + Mathf.Max(0, num_upgrade - amount) + " GOLD";

        }
        if (pickaxe_type.GetToolType().Contains("Iron")) {
            this.gameObject.transform.GetChild(0).GetChild(3).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "UPGRADE IN " + Mathf.Max(0, num_upgrade - amount) + " GOLD";

        }
    }

    public void UpgradeWeapon() {
        int iron_amt = inv.GetResourceCount(resource_types[1]);
        int gold_amt = inv.GetResourceCount(resource_types[2]);
        if (weapon_type.GetToolType().Contains("Copper")) {
            if (iron_amt >= 10) {
                inv.AddResources(resource_types[1], -10);
                // Set the weapon img
                this.gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().sprite = sprites[0];
                this.weapon_type.SetTool(tools[0], sprites[0]);
                this.gameObject.transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "UPGRADE IN " + Mathf.Max(0, num_upgrade - gold_amt) + " GOLD";
                this.gameObject.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Damages Players\n10 dmg/hit";
            } else {
                Debug.Log("Not enough funds...");
            }
            return;
        }
        if (weapon_type.GetToolType().Contains("Iron")) {
            if (gold_amt >= 10) {
                inv.AddResources(resource_types[2], -10);
                // Set the weapon img
                this.gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().sprite = sprites[1];
                this.weapon_type.SetTool(tools[1], sprites[1]);
                this.gameObject.transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "FULLY UPGRADED";
                this.gameObject.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Damages Players\n25 dmg/hit";
            } else {
                Debug.Log("Not enough funds...");
            }
            return;
        }
    }


    public void UpgradeAxe() {
        int iron_amt = inv.GetResourceCount(resource_types[1]);
        int gold_amt = inv.GetResourceCount(resource_types[2]);
        if (axe_type.GetToolType().Contains("Copper")) {
            if (iron_amt >= 10) {
                inv.AddResources(resource_types[1], -10);
                // Set the weapon img
                this.gameObject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite = sprites[2];
                this.axe_type.SetTool(tools[2], sprites[2]);
                this.gameObject.transform.GetChild(0).GetChild(2).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "UPGRADE IN " + Mathf.Max(0, num_upgrade - gold_amt) + " GOLD";
                this.gameObject.transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Chops Trees\n2 wood/hit";
            } else {
                Debug.Log("Not enough funds...");
            }
            return;
        }
        if (axe_type.GetToolType().Contains("Iron")) {
            if (gold_amt >= 10) {
                inv.AddResources(resource_types[2], -10);
                // Set the weapon img
                this.gameObject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite = sprites[3];
                this.axe_type.SetTool(tools[3], sprites[3]);
                this.gameObject.transform.GetChild(0).GetChild(2).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "FULLY UPGRADED";
                this.gameObject.transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Chops Trees\n3 wood/hit";
            } else {
                Debug.Log("Not enough funds...");
            }
            return;
        }
    }

    public void UpgradePickaxe() {
        int iron_amt = inv.GetResourceCount(resource_types[1]);
        int gold_amt = inv.GetResourceCount(resource_types[2]);
        if (pickaxe_type.GetToolType().Contains("Copper")) {
            if (iron_amt >= 10) {
                inv.AddResources(resource_types[1], -10);
                // Set the weapon img
                this.gameObject.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Image>().sprite = sprites[4];
                this.pickaxe_type.SetTool(tools[4], sprites[4]);
                this.gameObject.transform.GetChild(0).GetChild(3).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "UPGRADE IN " + Mathf.Max(0, num_upgrade - gold_amt) + " GOLD";
                this.gameObject.transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Mines Ores\n2 ore/hit";
            } else {
                Debug.Log("Not enough funds...");
            }
            return;
        }
        if (pickaxe_type.GetToolType().Contains("Iron")) {
            if (gold_amt >= 10) {
                inv.AddResources(resource_types[2], -10);
                // Set the weapon img
                this.gameObject.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Image>().sprite = sprites[5];
                this.pickaxe_type.SetTool(tools[5], sprites[5]);
                this.gameObject.transform.GetChild(0).GetChild(3).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "FULLY UPGRADED";
                this.gameObject.transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Mines Ores\n3 ore/hit";
            } else {
                Debug.Log("Not enough funds...");
            }
            return;
        }
    }




}
