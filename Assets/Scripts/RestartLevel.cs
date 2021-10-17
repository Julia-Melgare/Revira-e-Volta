using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RestartLevel : MonoBehaviour
{
    public delegate void OnRestartLevel(InputValue input);
    public static OnRestartLevel onRestartLevel;

    void OnRestart(InputValue inputValue)
    {
        Debug.Log(inputValue);
        onRestartLevel?.Invoke(inputValue);
    }
}
