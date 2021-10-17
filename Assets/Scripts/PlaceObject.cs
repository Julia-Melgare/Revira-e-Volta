using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    private GameObject originalObject;

    public delegate void OnGrabEnd();
    public static OnGrabEnd onGrabEnd;

    private bool checkedAlready = false;

    public void setOriginalObject(GameObject originalObject)
    {
        this.originalObject = originalObject;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (checkedAlready)
            return;

        if(other.gameObject == originalObject)
        {
            other.transform.position = gameObject.transform.position;
            other.transform.rotation = gameObject.transform.rotation;

            other.attachedRigidbody.isKinematic = true;
            other.gameObject.layer = 0;


            for (int i = 0; i < other.transform.childCount; i++)
            {
                var child = other.transform.GetChild(i);
                child.gameObject.layer = 0;
            }


            //Debug.Log("ONGRAB END " + originalObject.name + " " + other.gameObject.name);
            checkedAlready = true;

            onGrabEnd?.Invoke();


            Destroy(gameObject);
        }
    }
}
