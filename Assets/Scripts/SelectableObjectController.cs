using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjectController : MonoBehaviour
{
    public Material silhouetteMaterial;
    private GameObject silhouette = null;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private float distanceThreashold = 0.1f;

    private void OnDisable()
    {
        GraberController.onGrabObject -= CheckSelected;
        LevelStateController.onStateChange -= StateChange;
    }

    private void OnEnable()
    {
        GraberController.onGrabObject += CheckSelected;
        LevelStateController.onStateChange += StateChange;

    }

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        gameObject.layer = 3;

        StartCoroutine(CheckSpawnSilhouette());
    }

    IEnumerator CheckSpawnSilhouette()
    {
        yield return new WaitForSeconds(0.2f);

        var dist = Vector3.Distance(originalPosition, transform.position);

        if (dist > distanceThreashold)
        {
            SpawnSilhouette();
            yield break;
        }


        yield return CheckSpawnSilhouette();

    }

    private void StateChange(LevelStateController.State gameState)
    {
        if(gameState == LevelStateController.State.Voltando)
            if(silhouette != null)
                silhouette.SetActive(true);
            else
            {
                GetComponent<Rigidbody>().isKinematic = true;
                gameObject.layer = 0;
            }
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

        silhouette.transform.position = originalPosition;
        silhouette.transform.rotation = originalRotation;

        silhouette.gameObject.layer = 0;


        Destroy(silhouette.GetComponent<Rigidbody>());
        Destroy(silhouette.GetComponent<Collider>());
        Destroy(silhouette.GetComponent<SelectableObjectController>());

        var silhouetteCollider = silhouette.AddComponent<BoxCollider>();
        silhouetteCollider.isTrigger = true;
        silhouetteCollider.size = silhouetteCollider.size * 2;
        silhouette.GetComponent<MeshRenderer>().material = silhouetteMaterial;

        Color color = gameObject.GetComponent<MeshRenderer>().material.color;
       
        var newColor = new Color(color.r, color.g, color.b, 0.35f);

        silhouette.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", newColor);


        var placeObjectScript = silhouette.AddComponent<PlaceObject>();
        placeObjectScript.setOriginalObject(this.gameObject);

        silhouette.SetActive(false);

    }

    public bool HasSilhouette()
    {
        return silhouette != null;
    }
}
