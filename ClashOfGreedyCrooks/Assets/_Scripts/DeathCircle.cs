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

	private ParticleSystem ps;
	public bool emit;
	private bool particleFade;

	[HideInInspector]
	public bool roundIsOver = false;

	private void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start()
	{
		ps = GetComponent<ParticleSystem>();

		baseScale = transform.localScale;
		transform.localScale = baseScale * startSize;
		currScale = startSize;
		targetScale = baseScale * startSize;
	}

	// Update is called once per frame
	void Update()
	{
		var emission = ps.emission;
		var main = ps.main;

		emission.enabled = emit;

		if (particleFade && startColorAplha <= 1)
		{
			StartCircleFadeIn();
			main.startColor = new Color(255, 255, 255, startColorAplha);
		}

		if (!roundIsOver)
		{
			transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, contractionSpeed * Time.deltaTime);
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
		}
		currScale = Mathf.Clamp(currScale, minSize, maxSize + 1);

		targetScale = baseScale * currScale;

		emit = true;
		particleFade = true;
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

