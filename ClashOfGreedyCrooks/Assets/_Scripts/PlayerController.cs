using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]public Shooting shootS;

	public float speed = 5f;
	private float aimAngle = 0f;
	private float dt = 0f;

	private Vector3 movement;
	private Vector3 directionalInputLeftStick;
	private Vector3 directionalInputRightStick;


	private void Start()
	{
		directionalInputLeftStick = Vector3.zero;
		directionalInputRightStick = Vector3.zero;
		movement = Vector3.zero;

        shootS = GetComponent<Shooting>();
	}

	private void Update()
	{
		dt = Time.deltaTime;

		MovePlayer();
	}

	private void MovePlayer()
	{
		movement = new Vector3(directionalInputLeftStick.x, 0f, directionalInputLeftStick.z);
		movement = movement.normalized * speed * dt;
		transform.position += movement;

		aimAngle = Mathf.Atan2(directionalInputRightStick.x, directionalInputRightStick.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.up);
	}

	public void Shoot()
	{
        shootS.Shoot();
	}

	public void SetDirectionalInput(Vector3 leftStick, Vector3 rightStick)
	{
		directionalInputLeftStick = leftStick;

		if (Mathf.Abs(rightStick.x) > 0.19f || Mathf.Abs(rightStick.z) > 0.19f)
		{
			directionalInputRightStick = rightStick;
		}
	}


	//------------
	//   public float speed;
	//   public int playernumber;
	//   private float moveHorizontal;
	//   private float moveVertical;

	//   private Rigidbody rb;

	//// Use this for initialization
	//void Start () {
	//       rb = GetComponent<Rigidbody>();
	//}

	//// Update is called once per frame
	//void FixedUpdate ()
	//   {
	//       if (playernumber == 1)
	//       {
	//           moveHorizontal = Input.GetAxis("Horizontal");
	//           moveVertical = Input.GetAxis("Vertical");
	//       }else if (playernumber == 2)
	//       {
	//           moveHorizontal = Input.GetAxis("Horizontal2");
	//           moveVertical = Input.GetAxis("Vertical2");
	//       }

	//       Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

	//       transform.position += movement * speed;
	//   }
	//--------------- 
}
