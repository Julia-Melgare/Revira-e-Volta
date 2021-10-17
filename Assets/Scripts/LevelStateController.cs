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

    public delegate void OnSetTimer(float totalTime);
    public static OnSetTimer onSetTimer;

    public delegate void OnGameWin(bool win);
    public static OnGameWin onGameWin;

    private bool playerHasObject;

    [SerializeField] private bool isVoltaRequired;
    [SerializeField] private bool isReviraRequired;

    [SerializeField] private State gameState;

    [SerializeField] private int voltaTotalObjects;
    [SerializeField] private int currentVoltaObjects;

    [SerializeField] private float Score;
    [SerializeField] private float Time;

    private float timeRemaining;


    private void Start()
    {
        timeRemaining = this.Time;
        onSetTimer?.Invoke(this.Time);
    }

    private void Update()
    {
        timeRemaining -= UnityEngine.Time.deltaTime;
        if(timeRemaining <= 0)
        {
            onGameWin?.Invoke(false);
        }
    }

    void OnEnable()
    {
        GraberController.onGrabGoal += GrabGoal;
        ExitPoint.playerExit += PlayerTryExit;
        PlaceObject.onGrabEnd += OnVoltaObject;
    }

    private void OnDisable()
    {
        GraberController.onGrabGoal -= GrabGoal;
        ExitPoint.playerExit -= PlayerTryExit;
        PlaceObject.onGrabEnd -= OnVoltaObject;
    }

    private void PlayerTryExit()
    {

        if (gameState == State.End)
        {
            //END LEVEL
            Debug.Log("LEVEL END");
            onGameWin?.Invoke(true);

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
            SetVoltaTotalObjects();
            if(voltaTotalObjects == 0)
            {
                gameState = State.End;
                onStateChange?.Invoke(gameState);
            }


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

    private void SetVoltaTotalObjects()
    {
        List<GameObject> selectableObjects = GetSelectableObjects();
        foreach(GameObject selectable in selectableObjects)
        {
            var selectableController = selectable.GetComponent<SelectableObjectController>();
            if (selectableController != null)
            {
                if (selectableController.HasSilhouette())
                {
                    voltaTotalObjects++;
                }
            }
        }
    }

    private void OnVoltaObject()
    {
        currentVoltaObjects++;

        if (currentVoltaObjects < voltaTotalObjects)
            return;

        if (gameState == State.Voltando)
        {
            gameState = State.End;
            onStateChange?.Invoke(gameState);
        }

    }

    private static List<GameObject> GetObjectsInLayer(GameObject root, int layer)
    {
        var ret = new List<GameObject>();
        foreach (Transform t in root.transform.GetComponentsInChildren(typeof(GameObject), true))
        {
            if (t.gameObject.layer == layer)
            {
                ret.Add(t.gameObject);
            }
        }
        return ret;
    }

    private static List<GameObject> GetSelectableObjects()
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new System.Collections.Generic.List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == 3)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList;
    }

}
