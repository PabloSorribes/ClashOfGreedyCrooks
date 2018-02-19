using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates a bullet in the direction that the player is facing.
/// </summary>
public class Shooting : MonoBehaviour
{
	private Weapon weapon;

	FMODUnity.StudioEventEmitter a_shoot;

	//TODO: Input should come through the PlayerController / which button activates it should be derived from the PlayerController.
	private void Start()
	{
		InitializeAudio();
		weapon = GetComponentInChildren<Weapon>();
	}

	private void InitializeAudio()
	{
		a_shoot = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
		a_shoot.Event = "event:/Arena/playerShoot";
		a_shoot.Preload = true;
	}

	public void Shoot()
	{
		a_shoot.Play();

		weapon.InstantiateBullet(gameObject);
	}

	
}
