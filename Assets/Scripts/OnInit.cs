using UnityEngine;
using System.Collections;

public class OnInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameManager.Instance.HUD = gameObject;
	}
	
}
