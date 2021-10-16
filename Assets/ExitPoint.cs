using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExitPoint : MonoBehaviour
{

    public delegate void OnPlayerExit();
    public static OnPlayerExit playerExit;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player try exit");
            playerExit?.Invoke();
        }
    }

}
