using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    [field: SerializeField] public int gameStartScene;

    public void StartGameHost()
    {
        PlayerPrefs.SetInt("type", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameStartScene);
    }

    public void StartGameClient()
    {
        PlayerPrefs.SetInt("type", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameStartScene);
    }

    public void LoadMenu() {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene(0);
    }
}
