using UnityEngine;
using System.Collections;

public class InitWest : MonoBehaviour {

	public Grid grid;
	
	// Use this for initialization
	void Start () {
		grid = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<Grid> ();
		transform.localScale = new Vector3 (1, 2, grid.height);
		transform.position = new Vector3 (-grid.width / 2, 0f, grid.radius / 2);
	}
}
