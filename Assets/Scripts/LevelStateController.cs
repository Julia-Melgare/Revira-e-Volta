using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStateController : MonoBehaviour
{
    // Start is called before the first frame update

    public enum State
    {
        None, Revirando, Voltando, End
    }

    public delegate void OnStateChange(State gameState);
    public static OnStateChange onStateChange;


    private bool playerHasObject;

    private bool isVoltaRequired;
    private bool isReviraRequired;

    [SerializeField] private State gameState;

    [SerializeField] private int voltaTotalObjects;
    [SerializeField] private int currentVoltaObjects;

    [SerializeField] private float Score;
    [SerializeField] private float Time;


    private void Start()
    {
       
    }

    void OnEnable()
    {
        GoalObjectController.onGrabGoal += GrabGoal;
        ExitPoint.playerExit += PlayerTryExit;
    }

    private void PlayerTryExit()
    {

        if (gameState == State.End)
        {
            //END LEVEL
            Debug.Log("LEVEL END");
            
        }
        else
        {
            Debug.Log("DIDNT COMPLETE YET");
        }
    }

    private void GrabGoal()
    {
        Debug.Log("GRABED GOAL");

        if (gameState == State.Revirando && isVoltaRequired)
        {
            gameState = State.Voltando;
            onStateChange?.Invoke(gameState);
        }
        else if (gameState == State.Revirando && !isVoltaRequired)
        {
            gameState = State.End;
            onStateChange?.Invoke(gameState);
        }

        else if (gameState == State.Voltando)
        {
            // here only case for the onboarding
            gameState = State.End;
            onStateChange?.Invoke(gameState);
        }

    }

    private void OnVoltaObject()
    {
        currentVoltaObjects++;

        if (currentVoltaObjects < voltaTotalObjects)
            return;

        if(gameState == State.Voltando)
        {
            gameState = State.End;
            onStateChange?.Invoke(gameState);
        }
        
    }

 
}
