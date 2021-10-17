using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MultiSceneManager : MonoBehaviour
{
    // Start is called before the first frame update

    private static int sceneID;

    private void OnEnable()
    {
        RestartLevel.onRestartLevel += OnRestart;
    }

    private void OnDisable()
    {
        RestartLevel.onRestartLevel -= OnRestart;
    }

    void Start()
    {
        //sceneID = PlayerPrefs.GetInt("Level", 3);
        LoadLevel(PlayerPrefs.GetInt("Level", 3));
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
    }

    void OnRestart(InputValue inputValue)
    {
        Reload();
    }

    public void Reload()
    {
        var active = SceneManager.GetActiveScene();
        LoadLevel(active.buildIndex);
    }

    public void LoadLevel(int ID)
    {
        SceneManager.LoadScene(ID);

        SceneManager.LoadScene("Player", LoadSceneMode.Additive);

        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }
}
