using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public List<PlayerController> playerControllers;

    private GameManager gameMan;
    private FocusLevel focusLevel;
    
    //public float horizontalMove = 5f;
    //public float verticalMove = 7f;
    //public float depthMove = 5f;
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

	
    
    // Use this for initialization
	void Start ()
    {
        focusLevel = FindObjectOfType<FocusLevel>();
        gameMan = GameManager.GetInstance();
	}
	
	// Update is called once per frame
	private void LateUpdate ()
    {
        CalculateCameraLocations();
        MoveCamera();
	}

    public void CalculateCameraLocations()
    {
        Vector3 avarageCenter = Vector3.zero;
        Vector3 totalPositions = Vector3.zero;
        Bounds playerBounds = new Bounds();

        for (int _index = 0; _index < playerControllers.Count; _index++)
        {
            Vector3 playerPosition = playerControllers[_index].transform.position;
            if(!focusLevel.focusBounds.Contains(playerPosition))
            {
                float playerX = Mathf.Clamp(playerPosition.x, focusLevel.focusBounds.min.x, focusLevel.focusBounds.max.x);
                float playerY = Mathf.Clamp(playerPosition.y, focusLevel.focusBounds.min.y, focusLevel.focusBounds.max.y);
                float playerZ = Mathf.Clamp(playerPosition.z, focusLevel.focusBounds.min.z, focusLevel.focusBounds.max.z);

            }
            totalPositions += playerPosition;
            playerBounds.Encapsulate(playerPosition);
        }

        avarageCenter = (totalPositions / playerControllers.Count);

        float extents = (playerBounds.extents.x + playerBounds.extents.y + playerBounds.extents.z);
        float lerpProcent = Mathf.InverseLerp(0, (focusLevel.halfXBounds + focusLevel.halfYBounds) / 2, extents);

        float depth = Mathf.Lerp(depthMax, depthMin, lerpProcent);
        float angle = Mathf.Lerp(angleMax, angleMin, lerpProcent);

        cameraEulerX = angle;
        cameraPosition = new Vector3(avarageCenter.x, avarageCenter.y, depth);
    }


    private void MoveCamera()
    {
        Vector3 position = gameObject.transform.position;

        if(position != cameraPosition)
        {
            Vector3 targetPosition = Vector3.zero;
            targetPosition.x = Mathf.MoveTowards(position.x, cameraPosition.x, positionUpdateSpeed * Time.deltaTime);
            targetPosition.y = Mathf.MoveTowards(position.y, cameraPosition.y + offsetY, positionUpdateSpeed * Time.deltaTime);
            targetPosition.z = Mathf.MoveTowards(position.z, cameraPosition.z, depthUpdateSpeed * Time.deltaTime);
            gameObject.transform.position = targetPosition;

        }

        Vector3 localEulerAngles = gameObject.transform.localEulerAngles;

        if(localEulerAngles.x != cameraEulerX)
        {
            Vector3 targetEulerAngles = new Vector3(cameraEulerX, localEulerAngles.y, localEulerAngles.z);
            gameObject.transform.localEulerAngles = Vector3.MoveTowards(localEulerAngles, targetEulerAngles, angleUpdateSpeed * Time.deltaTime);
        }
    }
    
}
