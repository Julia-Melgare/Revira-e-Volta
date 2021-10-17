using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject LoseScreen;

    private void OnEnable()
    {
        LevelStateController.onGameWin += CheckWin;
    }

    private void OnDisable()
    {
        LevelStateController.onGameWin -= CheckWin;
    }

    private void CheckWin(bool win)
    {
        if (win)
        {
            WinScreen.SetActive(true);
        }
        else
        {
            LoseScreen.SetActive(true);
        }
    }
}
