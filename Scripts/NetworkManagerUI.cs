using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    
    [SerializeField] private GameObject netUI;
    [SerializeField] private GameObject loadingUI;
    [SerializeField] bool is_server;

    private void Awake() {
        serverBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
            netUI.SetActive(false);
            loadingUI.SetActive(true);
        });

        hostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            netUI.SetActive(false);
            loadingUI.SetActive(true);
        });

        clientBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            netUI.SetActive(false);
            loadingUI.SetActive(true);
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        int what_type = PlayerPrefs.GetInt("type", 1);
        if (what_type == 0) {
            NetworkManager.Singleton.StartHost();
        } else {
            NetworkManager.Singleton.StartClient();
        }
        netUI.SetActive(false);
        loadingUI.SetActive(true);
        /*// Server Code
        if (is_server) {
            NetworkManager.Singleton.StartServer();
            Debug.Log("Starting Server...");
            
        } else {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Starting Client...");
        }
        netUI.SetActive(false);
        loadingUI.SetActive(true);
        // Client Code
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
