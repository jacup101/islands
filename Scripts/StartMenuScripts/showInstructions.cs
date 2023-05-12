using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showInstructions : MonoBehaviour
{
    [field: SerializeField] private GameObject mainPanel;
    [field: SerializeField] private GameObject instructionsPanel;

    public void hidePanel() {
        mainPanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }
}
