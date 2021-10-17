using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiSceneManager : MonoBehaviour
{
    // Start is called before the first frame update

    private static int sceneID;

    void Start()
    {

        sceneID = PlayerPrefs.GetInt("Level", 3);

        SceneManager.LoadSceneAsync(sceneID);

        SceneManager.LoadSceneAsync("Player", LoadSceneMode.Additive);

        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);


        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
