using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [field: SerializeField] private GameObject objective1;
    [field: SerializeField] private GameObject objective2;
    [field: SerializeField] private GameObject objective3;
    [field: SerializeField] private GameObject objective4;
    [field: SerializeField] private GameObject tutorialScreen;
    [field: SerializeField] private GameObject tutorialText;

    private int popUpIndex;
    private bool obj1 = false;
    private bool obj2 = false;
    private bool obj3 = false;
    private bool obj4 = false;

    private bool tutorialOn = true;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E)) {
            tutorialOn = !tutorialOn;
            tutorialScreen.SetActive(tutorialOn);
        }
        UpdateText(popUpIndex);
        switch(popUpIndex)
        {
            case 0:
                if(Input.GetKeyDown(KeyCode.E)) {
                    objective1.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    obj1 = true;
                }
                if(Input.GetKeyDown(KeyCode.E) & tutorialOn) {
                    objective2.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    obj2 = true; 
                    obj3 = true;
                    obj4 = true;
                }
                break;
            // Movement Controls
            case 1:
                if (Input.GetKeyDown(KeyCode.W)) {
                    objective1.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    obj1 = true;
                 }
                if (Input.GetKeyDown(KeyCode.S)) {
                    objective2.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    obj2 = true;
                }
                if (Input.GetKeyDown(KeyCode.A)) {
                    objective3.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    obj3 = true;
                }
                if (Input.GetKeyDown(KeyCode.D)) {
                    objective4.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    obj4 = true;
                }
                break;
            // Accessing Tools
            case 2:
                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    objective1.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    obj1 = true;
                 }
                if (Input.GetKeyDown(KeyCode.Alpha2)) {
                    objective2.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    obj2 = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3)) {
                    objective3.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    obj3 = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha4)) {
                    objective4.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    obj4 = true;
                }
                break;
            // Using Axe
            case 3:
                if (Input.GetKeyDown(KeyCode.Alpha2)) {
                    objective1.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    obj1 = true;
                }
                break;

        }

        if (obj1 & obj2 & obj3 & obj4)
        {
            tutorialOn = true;
            tutorialScreen.SetActive(tutorialOn);
            UpdateIndex();
        } 
    }


    private void UpdateText(int popUpIndex)
    {
        // Handle Objective Text
        switch(popUpIndex)
        {
            case 0:
                objective1.GetComponent<TextMeshProUGUI>().text = "Close Tutorial Menu";
                objective2.GetComponent<TextMeshProUGUI>().text = "Open Tutorial Menu";
                objective3.GetComponent<TextMeshProUGUI>().text = "";
                objective4.GetComponent<TextMeshProUGUI>().text = "";
                tutorialText.GetComponent<TextMeshProUGUI>().text = 
@"Welcome to Islandz.IO!\n
In this tutorial, we'll cover the basics.
- Movement
- Using Tools
- Mining
- Exploration

To toggle the tutorial menu, press E.";
                break;
            case 1:
                objective1.GetComponent<TextMeshProUGUI>().text = "Move Up";
                objective2.GetComponent<TextMeshProUGUI>().text = "Move Down";
                objective3.GetComponent<TextMeshProUGUI>().text = "Move Left";
                objective4.GetComponent<TextMeshProUGUI>().text = "Move Right";
                tutorialText.GetComponent<TextMeshProUGUI>().text = 
@"That was an easy one!
Now, let's get into Movement.
To move up, press the W key.
To move down, press the S key.
To move left, press the L key.
To move right, press the R key.";
                break;
            case 2:
                objective1.GetComponent<TextMeshProUGUI>().text = "Equip Weapon";
                objective2.GetComponent<TextMeshProUGUI>().text = "Equip Axe";
                objective3.GetComponent<TextMeshProUGUI>().text = "Equip Pick";
                objective4.GetComponent<TextMeshProUGUI>().text = "Equip Bridge";
                tutorialText.GetComponent<TextMeshProUGUI>().text = 
@"There are 4 tools needed in Islandz.
They are the weapon, axe, pick, and bridge tools.
We will cover what they do soon.
First, to equip a tool you can use hotkeys 1-4
or, you can click the tool in the bottom panel.";
                break;
            case 3:
                objective1.GetComponent<TextMeshProUGUI>().text = "Equip Axe";
                objective2.GetComponent<TextMeshProUGUI>().text = "Walk to Tree";
                objective3.GetComponent<TextMeshProUGUI>().text = "Use Axe";
                objective4.GetComponent<TextMeshProUGUI>().text = "Collect Wood";
                break;
            default:
                break;
        }
    }

    private void UpdateIndex()
    {
        ResetObjectives();
        popUpIndex++;
    }

    private void ResetObjectives()
    {
        objective1.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        objective2.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        objective3.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        objective4.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;

        obj1 = false;
        obj2 = false;
        obj3 = false;
        obj4 = false;
    }

    public void CompleteObjective(int id)
    {
        switch(id)
        {
            case 2:
                obj2 = true;
                break;
            default:
                break;
        }
    }
}
