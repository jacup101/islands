using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ResourceCounts : MonoBehaviour
{
    [field: SerializeField] private GameObject rockText {get; set;}
    [field: SerializeField] private GameObject woodText {get; set;}
    [field: SerializeField] private GameObject goldText {get; set;}
    [field: SerializeField] private GameObject crystalText {get; set;}

    [field: SerializeField] private GameObject wood_Resource;
    [field: SerializeField] private GameObject rock_Resource;
    [field: SerializeField] private GameObject gold_Resource;
    [field: SerializeField] private GameObject crystal_Resource;

    [field: SerializeField] public Material bright;
    [field: SerializeField] public InventoryMenu invMenu;



    public void UpdateResourceCount(Resource type, int count)
    {
        switch(type.ToString())
        {
            case "Resource_Rock (Resource)":
                rockText.GetComponent<TextMeshProUGUI>().text = count.ToString();
                invMenu.SetIronText(count);
                break;
            case "Resource_Wood_Brown (Resource)":
                woodText.GetComponent<TextMeshProUGUI>().text = count.ToString();
                break;
            case "Resource_Gold (Resource)":
                goldText.GetComponent<TextMeshProUGUI>().text = count.ToString();
                invMenu.SetGoldText(count);
                break;
            case "Resource_Crystal (Resource)":
                crystalText.GetComponent<TextMeshProUGUI>().text = count.ToString();
                break;
            default:
                break;
        }
        
    }

    public void AllowToolUpdate(Resource type)
    {
        switch((type.ToString()))
        {
            case "Resource_Rock (Resource)":
                rock_Resource.GetComponent<Image>().material = bright;
                break;
            case "Resource_Wood_Brown (Resource)":
                wood_Resource.GetComponent<Image>().material = bright;
                break;
            case "Resource_Gold (Resource)":
                gold_Resource.GetComponent<Image>().material = bright;
                break;
            case "Resource_Crystal (Resource)":
                crystal_Resource.GetComponent<Image>().material = bright;
                break;
            default:
                break;
        }
    }
    public void DisableToolUpdate(Resource type)
    {
        switch((type.ToString()))
        {
            case "Resource_Rock (Resource)":
                rock_Resource.GetComponent<Image>().material = null;
                break;
            case "Resource_Wood_Brown (Resource)":
                wood_Resource.GetComponent<Image>().material = null;
                break;
            case "Resource_Gold (Resource)":
                gold_Resource.GetComponent<Image>().material = null;
                break;
            case "Resource_Crystal (Resource)":
                crystal_Resource.GetComponent<Image>().material = null;
                break;
            default:
                break;
        }
    }
}
