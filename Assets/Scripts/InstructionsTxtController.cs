using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsTxtController : MonoBehaviour
{
    [SerializeField] private List<GameObject> instructionsTexts;
    private GameObject currentInstructions;

    private void OnEnable()
    {
        LevelStateController.onStateChange += StateChange;
    }

    private void Start()
    {
        currentInstructions = instructionsTexts[0];
        currentInstructions.SetActive(true);
    }

    private void StateChange(LevelStateController.State gameState)
    {
        switch (gameState)
        {
            case LevelStateController.State.Voltando:
                currentInstructions.SetActive(false);
                instructionsTexts[1].SetActive(true);
                currentInstructions = instructionsTexts[1];
                break;
            case LevelStateController.State.End:
                currentInstructions.SetActive(false);
                instructionsTexts[2].SetActive(true);
                currentInstructions = instructionsTexts[2];
                break;
        }
    }
}
