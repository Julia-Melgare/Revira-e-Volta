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

    public delegate void OnObjectCollision(Collision other);
    public static OnObjectCollision onObjectCollision;

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

        var silhouetteRender = silhouette.GetComponent<MeshRenderer>();
        var silhouetteMats = new Material[silhouetteRender.materials.Length];
        for (int j = 0; j < silhouetteRender.materials.Length; j++)
        {
            silhouetteMats[j] = silhouetteMaterial;
        }
        silhouetteRender.materials = silhouetteMats;

        for (int i = 0; i < silhouette.transform.childCount; i++)
        {
            var child = silhouette.transform.GetChild(i);

            var collider = child.GetComponent<Collider>();
            var renderer = child.GetComponent<Renderer>();

            if(collider)
                Destroy(collider);
            if (renderer)
            {
                var mats = new Material[renderer.materials.Length];
                for (int j = 0; j < renderer.materials.Length; j++)
                {
                    mats[j] = silhouetteMaterial;
                }
                renderer.materials = mats;
            }
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

    private void OnCollisionEnter(Collision collision)
    {
        onObjectCollision?.Invoke(collision);
        Debug.Log(collision.gameObject.name);
    }
}
