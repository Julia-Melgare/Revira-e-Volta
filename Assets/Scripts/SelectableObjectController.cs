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


    private bool createdSilhouette = false;

    private void OnDisable()
    {
        //GraberController.onGrabObject -= CheckSelected;
        LevelStateController.onStateChange -= StateChange;
    }

    private void OnEnable()
    {
        //GraberController.onGrabObject += CheckSelected;
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
            createdSilhouette = true;
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
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    var child = gameObject.transform.GetChild(i);
                    child.gameObject.layer = 0;
                }
            }
    }

    //private void CheckSelected(GameObject selected)
    //{
    //    if(selected == this.gameObject && silhouette == null)
    //    {
    //        SpawnSilhouette();
    //    }
    //}

    private void SpawnSilhouette()
    {
        silhouette = Instantiate(gameObject);

        //silhouette.transform.SetParent(transform.parent);


        silhouette.transform.position = originalPosition;
        silhouette.transform.rotation = originalRotation;

        silhouette.gameObject.layer = 0;

        Destroy(silhouette.GetComponent<Rigidbody>());
        Destroy(silhouette.GetComponent<Collider>());
        Destroy(silhouette.GetComponent<SelectableObjectController>());

        silhouette.GetComponent<MeshRenderer>().material = silhouetteMaterial;

        //Color color = gameObject.GetComponent<MeshRenderer>().material.color;

        //var newColor = new Color(color.r, color.g, color.b, 0.35f);

        //silhouette.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", newColor);


        for (int i = 0; i < silhouette.transform.childCount; i++)
        {
            var child = silhouette.transform.GetChild(i);

            var collider = child.GetComponent<Collider>();
            var renderer = child.GetComponent<Renderer>();

            if(collider)
                Destroy(collider);
            if (renderer)
                renderer.material = silhouetteMaterial;

        }

        var silhouetteCollider = silhouette.AddComponent<BoxCollider>();
        silhouetteCollider.isTrigger = true;
        silhouetteCollider.size = silhouetteCollider.size * 2;   
                     
        var placeObjectScript = silhouette.AddComponent<PlaceObject>();
        placeObjectScript.setOriginalObject(gameObject);

        silhouette.tag = "Sil";
        silhouette.SetActive(false);

    }

    public bool HasSilhouette()
    {
        return createdSilhouette;
    }
}
