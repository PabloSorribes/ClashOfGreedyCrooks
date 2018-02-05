using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AkSoundEngine.PostEvent("ButtonAccept", this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
