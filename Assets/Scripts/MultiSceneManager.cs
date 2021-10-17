using System;
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
        LevelStateController.onGameWin += OnWin;

    }

    private void OnWin(bool win)
    {
        StartCoroutine(WinDelay());
    }

    IEnumerator WinDelay()
    {

        yield return new WaitForSeconds(5f);
        sceneID++;

        PlayerPrefs.SetInt("Level", sceneID);

        LoadLevel(sceneID);
    }
    

    /*private void OnDisable()
    {
        RestartLevel.onRestartLevel -= OnRestart;
    }*/

    void Start()
    {
        Scene manager = SceneManager.GetSceneByName("SceneManager");

        DontDestroyOnLoad(gameObject);

        sceneID = PlayerPrefs.GetInt("Level", 3);
        LoadLevel(sceneID);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
    }

    void OnRestart(InputValue inputValue)
    {
        Reload();
    }

    void ReloadSelf()
    {

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
