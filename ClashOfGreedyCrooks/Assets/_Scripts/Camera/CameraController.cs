using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
    private PlayerController[] playerControllers;

    private GameManager gameMan;
    private FocusLevel focusLevel;
    
    public float positionUpdateSpeed = 5f;
    public float depthUpdateSpeed = 5f;
    public float angleUpdateSpeed = 7f;

    public float depthMax = 25f;
    public float depthMin = -25f;

    public float angleMax = 0f;
    public float angleMin = 0f;

    private float cameraEulerX;
    private float cameraEulerY;

    private Vector3 cameraPosition;

    public float offsetY = 30f;
    public float offsetZ = 30f;

    public float maxZoom = 10f;
    public float minZoom = 40f;
    public float zoomLimiter = 50f;

    private Camera cam;

	
    
    // Use this for initialization
	void Start ()
    {
        cam = GetComponent<Camera>();

       playerControllers = FindObjectsOfType<PlayerController>();

        foreach (var playerobject in playerControllers)
        {
            Vector3 playerPosition = playerobject.transform.position;
        }

        focusLevel = FindObjectOfType<FocusLevel>();
        gameMan = GameManager.GetInstance;
	}
	
	// Update is called once per frame
	private void LateUpdate ()
    {
        CalculateCameraLocations();
        MoveCamera();
        Zoom();
	}

    public void CalculateCameraLocations()
    {
        Vector3 avarageCenter = Vector3.zero;
        Vector3 totalPositions = Vector3.zero;
        Bounds playerBounds = new Bounds();

        for (int i = 0; i < playerControllers.Length; i++)
        {
            Vector3 playerPosition = playerControllers[i].transform.position;
            if(!focusLevel.focusBounds.Contains(playerPosition))
            {
                float playerX = Mathf.Clamp(playerPosition.x, focusLevel.focusBounds.min.x, focusLevel.focusBounds.max.x);
                float playerY = Mathf.Clamp(playerPosition.y, focusLevel.focusBounds.min.y, focusLevel.focusBounds.max.y);
                float playerZ = Mathf.Clamp(playerPosition.z, focusLevel.focusBounds.min.z, focusLevel.focusBounds.max.z);

            }
            totalPositions += playerPosition;
            playerBounds.Encapsulate(playerPosition);
        }

        avarageCenter = (totalPositions / playerControllers.Length);

        float extents = (playerBounds.extents.x + playerBounds.extents.y + playerBounds.extents.z);
        float lerpProcent = Mathf.InverseLerp(0, (focusLevel.halfXBounds + focusLevel.halfYBounds) / 2, extents);

        float depth = Mathf.Lerp(depthMax, depthMin, lerpProcent);
        float angle = Mathf.Lerp(angleMax, angleMin, lerpProcent);

        cameraEulerX = angle;
        cameraPosition = new Vector3(avarageCenter.x, avarageCenter.y, avarageCenter.z);
    }

    private void MoveCamera()
    {
        Vector3 position = gameObject.transform.position;
        

        if(position != cameraPosition)
        {
            Vector3 targetPosition = Vector3.zero;
            targetPosition.x = Mathf.MoveTowards(position.x, cameraPosition.x, positionUpdateSpeed * Time.deltaTime);
            targetPosition.y = Mathf.MoveTowards(position.y, cameraPosition.y + offsetY, positionUpdateSpeed * Time.deltaTime);
            targetPosition.z = Mathf.MoveTowards(position.z, cameraPosition.z + offsetZ, positionUpdateSpeed * Time.deltaTime);
            gameObject.transform.position = targetPosition;

        }
        


        //Vector3 localEulerAngles = gameObject.transform.localEulerAngles;

        /*if(localEulerAngles.x != cameraEulerX)
        {
            Vector3 targetEulerAngles = new Vector3(cameraEulerX, localEulerAngles.y, localEulerAngles.z);
            gameObject.transform.localEulerAngles = Vector3.MoveTowards(localEulerAngles, targetEulerAngles, angleUpdateSpeed * Time.deltaTime);
        }*/
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, focusLevel.focusBounds.size.x / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }
    
}
