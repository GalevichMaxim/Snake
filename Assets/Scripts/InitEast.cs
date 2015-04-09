using UnityEngine;
using System.Collections;

public class InitEast : MonoBehaviour {

	public Grid grid;

	// Use this for initialization
	void Start () {
		grid = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<Grid> ();
		transform.localScale = new Vector3 (1, 1, grid.height);
		transform.position = new Vector3 (grid.width / 2 + 0.5f, 0f, grid.radius / 2);
	}
}
