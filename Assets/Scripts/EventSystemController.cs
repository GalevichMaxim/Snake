using UnityEngine;
using System.Collections;

public class EventSystemController : MonoBehaviour {

	void Awake () 
	{
		GameObject es = GameObject.FindGameObjectWithTag ("HUD");
		if (es != null)
		{
			//es.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
