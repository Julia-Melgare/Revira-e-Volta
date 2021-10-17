using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObjectController : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private float movementForce;
    private bool isMovingTowardsCamera = false;
    private Rigidbody rb;

    private void OnEnable()
    {
        GraberController.onGrabGoal += GrabGoal;
    }

    private void OnDisable()
    {
        GraberController.onGrabGoal -= GrabGoal;
    }

    private void Start()
    {
        cam = Camera.main;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isMovingTowardsCamera)
        {
            rb.AddForce(cam.transform.forward * movementForce * Time.deltaTime, ForceMode.VelocityChange);
            Debug.Log(Vector3.Distance(gameObject.transform.position, cam.transform.forward));
            if (Vector3.Distance(gameObject.transform.position, cam.transform.forward) <= 3f)
            {
                isMovingTowardsCamera = false;
            }
        }
        
    }

    private void GrabGoal()
    {
        isMovingTowardsCamera = true;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onGrabGoal?.Invoke();
            StartCoroutine(moveTowardsCamera());            
            //Destroy(gameObject);
        }

    }*/

}
