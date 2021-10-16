using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    private GameObject originalObject;

    public delegate void OnGrabEnd();
    public static OnGrabEnd onGrabEnd;


    public void setOriginalObject(GameObject originalObject)
    {
        this.originalObject = originalObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == originalObject)
        {
            other.transform.position = gameObject.transform.position;
            other.transform.rotation = gameObject.transform.rotation;

            other.attachedRigidbody.isKinematic = true;
            other.gameObject.layer = 0;

            onGrabEnd?.Invoke();

            Destroy(gameObject);
        }
    }
}
