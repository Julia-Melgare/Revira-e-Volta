using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuitGame : MonoBehaviour
{
    private void OnQuit(InputValue input)
    {
        Application.Quit();
    }
}
