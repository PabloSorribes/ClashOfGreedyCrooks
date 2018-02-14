using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Used by menu-buttons to play the Move-sound (when focusing on different buttons).
/// </summary>
public class MainMenuAudio : MonoBehaviour, ISelectHandler
{
	public void OnSelect(BaseEventData eventData)
	{
		AudioManager.GetInstance.PlayOneShot("event:/MainMenu/menuMove");
	}
}
