using UnityEngine;
using System.Collections;

public class InitNorth : MonoBehaviour {

	public Grid grid;
	
	// Use this for initialization
	void Start () {
		grid = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<Grid> ();
		transform.localScale = new Vector3 (grid.width, 1, 1);
		transform.position = new Vector3 (0, 0f, grid.height / 2 + grid.radius / 2 + 0.5f);
	}
}
