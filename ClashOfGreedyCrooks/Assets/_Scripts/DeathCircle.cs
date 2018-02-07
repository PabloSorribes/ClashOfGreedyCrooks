using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Courtesy of: https://answers.unity.com/questions/582223/scaling-objects-over-time.html
/// </summary>
public class DeathCircle : MonoBehaviour
{
	//CODECHANGE: Removed the singleton inheritance and replaced with a simple static since global access is all we need.
	private static DeathCircle instance;
	public static DeathCircle GetInstance
	{
		get
		{
			return instance;
		}
	}

	private float startSize = 1;
    private float maxSize = 5;
    private float minSize = 0.3f;

    public int deathZoneDamage = 15;
    private float contractionSpeed = 2f;

    private Vector3 targetScale;
    private Vector3 baseScale;
    private float currScale;

    // Use this for initialization
    void Start()
    {
        baseScale = transform.localScale;
        transform.localScale = baseScale * startSize;
        currScale = startSize;
        targetScale = baseScale * startSize;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, contractionSpeed * Time.deltaTime);

        //DEBUG: To trigger the shrinking manualy
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSize(true);
        }
    }


    public void ChangeSize(bool shrink)
    {
        if (shrink)
        {
            currScale--;
        }
        currScale = Mathf.Clamp(currScale, minSize, maxSize + 1);

        targetScale = baseScale * currScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().insideDeathCircle = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Take DAMAGE!");
            other.GetComponent<PlayerHealth>().insideDeathCircle = false;
        }
    }
}

