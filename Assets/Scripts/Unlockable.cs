using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlockable : MonoBehaviour
{

    [SerializeField] private GameObject Key;
    [SerializeField] private Rigidbody Unlock;

    void Start()
    {
        Unlock.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

        if(other.gameObject == Key)
        {
            Unlock.isKinematic = false;
            Destroy(Key);
        }


    }
}
