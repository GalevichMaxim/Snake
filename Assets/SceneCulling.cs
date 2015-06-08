using UnityEngine;
using System.Collections;

public class SceneCulling : MonoBehaviour {

	void Update () {
		if(GameObject.Find("MainMenuScene"))
		{
			gameObject.renderer.enabled = false;
		}
	}
}
