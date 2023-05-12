using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnPress : MonoBehaviour
{
    [field: SerializeField] private GameObject floatingHealthPrefab;
    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("Showing damage...");
            ShowDamage();
       }
    }

    void ShowDamage() {
        if (floatingHealthPrefab) {
            floatingHealthPrefab.SetActive(true);
            floatingHealthPrefab.GetComponent<Animation>().Play();
        }
    }
}
