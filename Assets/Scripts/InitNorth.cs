using UnityEngine;
using System.Collections;

public class InitNorth : MonoBehaviour {

	public Grid grid;
	
	void Start () {
		grid = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<Grid> ();
		transform.localScale = new Vector3 (grid.width, 2, 1);
		transform.position = new Vector3 (0, 0f, grid.height / 2 + grid.radius / 2);
	}
}
