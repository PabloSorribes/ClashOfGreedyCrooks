using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public bool isShaking;//For other scripts to know if the camera is shaking

    public float intensity;

    private float shakeIntensity;
    private float shakeDecay;
    private Vector3 originalPos;
    private Quaternion OriginalRot;

    

    void Start()
    {
        isShaking = false;

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            DoShake();
        }


        if (shakeIntensity > 0)
        {
            transform.position = originalPos + Random.insideUnitSphere * shakeIntensity;
            transform.rotation = new Quaternion(OriginalRot.x + Random.Range(-shakeIntensity, shakeIntensity) * intensity,
                                      OriginalRot.y + Random.Range(-shakeIntensity, shakeIntensity) * intensity,
                                      OriginalRot.z + Random.Range(-shakeIntensity, shakeIntensity) * intensity,
                                      OriginalRot.w + Random.Range(-shakeIntensity, shakeIntensity) * intensity);

            shakeIntensity -= shakeDecay;
        }
        else if (isShaking)
        {
            isShaking = false;
        }
    }

    public void DoShake()
    {
        originalPos = transform.position;
        OriginalRot = transform.rotation;

        shakeIntensity = 0.3f;
        shakeDecay = 0.02f;
        isShaking = true;
    }


}