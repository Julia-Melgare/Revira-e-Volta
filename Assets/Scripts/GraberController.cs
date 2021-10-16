using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GraberController : MonoBehaviour
{


    [SerializeField] private float DragForce = 1f;
    [SerializeField] private float DampAmount = 1f;

    private Camera MainCamera;


    private Rigidbody selected;
    private float selectedMagnitude;

    private Transform joint;

    private bool mouseClick;

    public delegate void OnGrabObject(GameObject selected);
    public static event OnGrabObject onGrabObject;


    private void OnEnable()
    {
        PlaceObject.onGrabEnd += GrabEnd;
    }

    private void Awake()
    {
        MainCamera = Camera.main;
    }

    private void GrabEnd()
    {
        selected = null;
        if(joint)
            Destroy(joint.gameObject);
    }

    private void OnFire(InputValue value)
    {
        mouseClick = value.isPressed;

        if (mouseClick)
        {
            RaycastHit hit;

            if (Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hit, 10f, ~3, QueryTriggerInteraction.UseGlobal))
            {
                selected = hit.rigidbody;

                Vector3 camToSelected = selected.transform.position - MainCamera.transform.position;
                selectedMagnitude = camToSelected.magnitude;

                joint = CreateJoint(selected, hit.point);

                onGrabObject?.Invoke(hit.collider.gameObject);
                //selected.GetComponent<Renderer>().material.color = Color.red;
            }
        }
        else if (selected)
        {
            //selected.GetComponent<Renderer>().material.color = Color.white;
            selected = null;
            Destroy(joint.gameObject);
        }


    }

    private Transform CreateJoint(Rigidbody rigidbody, Vector3 attachPosition)
    {
        GameObject g = new GameObject("Attach");
        g.transform.position = attachPosition;
        //g.transform.localScale = Vector3.one / 5f;
        //g.GetComponent<Collider>().enabled = false;

        Rigidbody r = g.AddComponent<Rigidbody>();

        r.isKinematic = true;

        var joint = g.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rigidbody;
        joint.configuredInWorldSpace = true;

        JointDrive drive = new JointDrive { mode = JointDriveMode.PositionAndVelocity, maximumForce = Mathf.Infinity, positionDamper = DampAmount, positionSpring = DragForce };

        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;
        joint.slerpDrive = drive;
        joint.rotationDriveMode = RotationDriveMode.Slerp;

        return g.transform;

    }

    private void Update()
    {
        if (selected)
        {
            Vector3 selectedVector = MainCamera.transform.position - selected.transform.position;

            Vector3 mouseVector = Mouse.current.position.ReadValue();

            mouseVector = CameraPlane.ScreenToWorldPlanePoint(Camera.main, selectedMagnitude, mouseVector);

            Debug.DrawLine(MainCamera.transform.position, mouseVector, Color.red);
            Debug.DrawLine(MainCamera.transform.position, MainCamera.transform.position + selectedVector, Color.blue);

            if (joint)
            {
                joint.transform.position = mouseVector;
            }

        }
    }


}

public static class CameraPlane
{
    /// <summary>
    /// Returns world space position at a given viewport coordinate for a given depth.
    /// </summary>
    public static Vector3 ViewportToWorldPlanePoint(Camera theCamera, float zDepth, Vector2 viewportCord)
    {
        Vector2 angles = ViewportPointToAngle(theCamera, viewportCord);
        float xOffset = Mathf.Tan(angles.x) * zDepth;
        float yOffset = Mathf.Tan(angles.y) * zDepth;
        Vector3 cameraPlanePosition = new Vector3(xOffset, yOffset, zDepth);
        cameraPlanePosition = theCamera.transform.TransformPoint(cameraPlanePosition);
        return cameraPlanePosition;
    }

    public static Vector3 ScreenToWorldPlanePoint(Camera camera, float zDepth, Vector3 screenCoord)
    {
        var point = Camera.main.ScreenToViewportPoint(screenCoord);
        return ViewportToWorldPlanePoint(camera, zDepth, point);
    }

    /// <summary>
    /// Returns X and Y frustum angle for the given camera representing the given viewport space coordinate.
    /// </summary>
    public static Vector2 ViewportPointToAngle(Camera cam, Vector2 ViewportCord)
    {
        float adjustedAngle = AngleProportion(cam.fieldOfView / 2, cam.aspect) * 2;
        float xProportion = ((ViewportCord.x - .5f) / .5f);
        float yProportion = ((ViewportCord.y - .5f) / .5f);
        float xAngle = AngleProportion(adjustedAngle / 2, xProportion) * Mathf.Deg2Rad;
        float yAngle = AngleProportion(cam.fieldOfView / 2, yProportion) * Mathf.Deg2Rad;
        return new UnityEngine.Vector2(xAngle, yAngle);
    }

    /// <summary>
    /// Distance between the camera and a plane parallel to the viewport that passes through a given point.
    /// </summary>
    public static float CameraToPointDepth(Camera cam, Vector3 point)
    {
        Vector3 localPosition = cam.transform.InverseTransformPoint(point);
        return localPosition.z;
    }

    public static float AngleProportion(float angle, float proportion)
    {
        float oppisite = Mathf.Tan(angle * Mathf.Deg2Rad);
        float oppisiteProportion = oppisite * proportion;
        return Mathf.Atan(oppisiteProportion) * Mathf.Rad2Deg;
    }
}
