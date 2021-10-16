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
        LevelStateController.onStateChange += StateChange;

    }

    private void StateChange(LevelStateController.State gameState)
    {
        if(gameState == LevelStateController.State.Voltando)
            if(silhouette != null)
                silhouette.SetActive(true);
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
        silhouette.SetActive(false);
        Destroy(silhouette.GetComponent<Rigidbody>());
        Destroy(silhouette.GetComponent<Collider>());
        silhouette.gameObject.layer = 0;
        var silhouetteCollider = silhouette.AddComponent<BoxCollider>();
        silhouetteCollider.isTrigger = true;
        silhouetteCollider.size = silhouetteCollider.size * 2;
        silhouette.GetComponent<MeshRenderer>().material = silhouetteMaterial;

        Color color = gameObject.GetComponent<MeshRenderer>().material.color;
       
        var newColor = new Color(color.r, color.g, color.b, 0.35f);

        silhouette.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", newColor);


        Destroy(silhouette.GetComponent<SelectableObjectController>());
        var placeObjectScript = silhouette.AddComponent<PlaceObject>();
        placeObjectScript.setOriginalObject(this.gameObject);
    }
}
