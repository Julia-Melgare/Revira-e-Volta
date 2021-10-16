using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjectController : MonoBehaviour
{
    public Material silhouetteMaterial;
    private GameObject silhouette = null; 

    private void OnDisable()
    {
        GraberController.onGrabObject -= CheckSelected;
    }

    private void OnEnable()
    {
        GraberController.onGrabObject += CheckSelected;
    }

    private void CheckSelected(GameObject selected)
    {
        if(selected == this.gameObject && silhouette == null)
        {
            SpawnSilhouette();
        }
    }

    private void SpawnSilhouette()
    {
        silhouette = Object.Instantiate(this.gameObject);
        Destroy(silhouette.GetComponent<Rigidbody>());
        Destroy(silhouette.GetComponent<Collider>());
        silhouette.gameObject.layer = 0;
        var silhouetteCollider = silhouette.AddComponent<BoxCollider>();
        silhouetteCollider.isTrigger = true;
        silhouetteCollider.size = silhouetteCollider.size * 2;
        silhouette.GetComponent<MeshRenderer>().material = silhouetteMaterial;
        Destroy(silhouette.GetComponent<SelectableObjectController>());
        var placeObjectScript = silhouette.AddComponent<PlaceObject>();
        placeObjectScript.setOriginalObject(this.gameObject);
    }
}
