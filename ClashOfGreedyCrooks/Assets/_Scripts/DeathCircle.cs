using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Courtesy of: https://answers.unity.com/questions/582223/scaling-objects-over-time.html
/// </summary>
public class DeathCircle : MonoBehaviour
{
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

	private float startColorAplha = 0.1f;
	public float FadeInStrength;

	public int deathZoneDamage = 15;
	private float contractionSpeed = 2f;

	private Vector3 targetScale;
	private Vector3 baseScale;
	private float currScale;

    [Header("The radius of the DeathCircle")]
    public float radiusPsc = 30;
    private float radiusPs;

    public float minRadius;

    public ParticleSystem ps;
    public ParticleSystem psc;
	private bool emitPs;
	private bool particleFade;

    private bool startAfter;

	[HideInInspector]
	public bool roundIsOver = false;
    private bool emitPsc;

    private void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start()
	{
		//ps = GetComponent<ParticleSystem>();
        //psc = GetComponentInChildren<ParticleSystem>();

		baseScale = transform.localScale;
		transform.localScale = baseScale * startSize;
		currScale = startSize;
		targetScale = baseScale * startSize;

        radiusPs = radiusPsc;
	}

	// Update is called once per frame
	void Update()
	{
		var emission = ps.emission;
        var emissionPsc = psc.emission;
		var main = ps.main;

        var sizePsc = psc.shape;
        var sizePs = ps.shape;

        sizePsc.radius = radiusPsc;
        sizePs.radius = radiusPs;

		emission.enabled = emitPs;
        emissionPsc.enabled = emitPsc;

		if (particleFade && startColorAplha <= 1)
		{
			StartCircleFadeIn();
			main.startColor = new Color(255, 255, 255, startColorAplha);
		}

		if (!roundIsOver)
		{
			transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, contractionSpeed * Time.deltaTime);
		}

        if (startAfter)
        {
            AfterTrails();

            if (radiusPsc <= minRadius)
            {
                startAfter = false;
            }
        }

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
            startAfter = true;
		}
		currScale = Mathf.Clamp(currScale, minSize, maxSize + 1);

		targetScale = baseScale * currScale;


		emitPs = true;
        emitPsc = true;
		particleFade = true;
	}

    private void AfterTrails()
    {
        radiusPsc -= 0.7f * Time.deltaTime;
        radiusPs -= 0.7f * Time.deltaTime;
    }

	private void StartCircleFadeIn()
	{
		startColorAplha += startColorAplha * FadeInStrength * Time.deltaTime;
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

			//TODO: Make death circle activate an Invoke or a CoRoutine in PlayerHealth. 
			//Test if it still works when two players are outside, and one of them re-enters inside death circle.
			//other.GetComponent<PlayerHealth>().InvokeRepeating("TakeDamage")
		}
	}
}

