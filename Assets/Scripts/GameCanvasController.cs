using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private GameObject BigInstructions;
    [SerializeField] private GameObject SmallInstructions;
    [SerializeField] private GameObject VoltaItemsRemaining;

    private void Start()
    {
        StartCoroutine(ShowBigInstructions());
    }

    private void OnEnable()
    {
        LevelStateController.onGameWin += CheckWin;
        LevelStateController.onStateChange += StateChange;
    }

    private void OnDisable()
    {
        LevelStateController.onGameWin -= CheckWin;
        LevelStateController.onStateChange -= StateChange;
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

    private void StateChange(LevelStateController.State state)
    {
        SmallInstructions.SetActive(false);
        BigInstructions.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(ShowBigInstructions());
        if(state == LevelStateController.State.Voltando)
        {
            VoltaItemsRemaining.SetActive(true);
        }else if(state == LevelStateController.State.End)
        {
            VoltaItemsRemaining.SetActive(false);
        }
    }

    IEnumerator ShowBigInstructions()
    {
        yield return new WaitForSeconds(5f);
        BigInstructions.GetComponent<CanvasGroup>().alpha = 0;
        SmallInstructions.SetActive(true);
    }
}
