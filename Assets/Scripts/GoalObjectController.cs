using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObjectController : MonoBehaviour
{
    public delegate void OnGrabGoal();
    public static OnGrabGoal onGrabGoal;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

        if (other.CompareTag("Player"))
        {
            onGrabGoal?.Invoke();
            Destroy(gameObject);
        }

    }


}
