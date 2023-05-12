using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMain : MonoBehaviour
{
    [field: SerializeField] private GameObject mainMenu;
    [field: SerializeField] private GameObject aboutMenu;

    public void ReturnToMain()
    {
        aboutMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ShowAboutMenu() {
        mainMenu.SetActive(false);
        aboutMenu.SetActive(true);
    }
}
