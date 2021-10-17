using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public delegate void OnVoltaTotalObjectsReady(int value);
    public static OnVoltaTotalObjectsReady onVoltaTotalObjReady;

    public delegate void OnCurrentVoltaObjectsChange(int value);
    public static OnCurrentVoltaObjectsChange onCurrentVoltaObjChange;

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

            SetVoltaTotalObjects();

            onStateChange?.Invoke(gameState);
            
            if(voltaTotalObjects == 0)
            {
                gameState = State.End;
                onStateChange?.Invoke(gameState);
            }
            else
            {
                onVoltaTotalObjReady?.Invoke(voltaTotalObjects);
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

        //voltaTotalObjects = GameObject.FindGameObjectsWithTag("Sil").Length;

        List<SelectableObjectController> selectableObjects = GetSelectableObjects();
        foreach (SelectableObjectController selectable in selectableObjects)
        {
            if (selectable != null)
            {
                if (selectable.HasSilhouette())
                {

                    //Debug.LogWarning(selectable.gameObject.name + " HAS A SILHOUETE");
                    voltaTotalObjects++;
                }
                else
                {
                    //Debug.LogError(selectable.gameObject.name + " DOES NOT HAVE A SILHOUETE");

                }
            }
        }
    }

    private void OnVoltaObject()
    {
        currentVoltaObjects++;
        onCurrentVoltaObjChange?.Invoke(currentVoltaObjects);

        if (currentVoltaObjects < voltaTotalObjects)            
            return;         

        if (gameState == State.Voltando)
        {
            gameState = State.End;
            onStateChange?.Invoke(gameState);
        }

    }

    private static List<SelectableObjectController> GetSelectableObjects()
    {

        var comp = GameObject.FindObjectsOfType<SelectableObjectController>(includeInactive:false).ToList();

        return comp;

        //var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        //var goList = new System.Collections.Generic.List<GameObject>();
        //for (int i = 0; i < goArray.Length; i++)
        //{
        //    if (goArray[i].layer == 3)
        //    {
        //        goList.Add(goArray[i]);
        //    }
        //}
        //if (goList.Count == 0)
        //{
        //    return null;
        //}
        //return goList;
    }

}
