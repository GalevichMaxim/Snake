using UnityEngine;
using System.Collections;

public class InitGround : MonoBehaviour {

	private Grid grid;

	void Start () 
	{
		grid = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<Grid> ();
		transform.localScale = new Vector3(grid.width / 10, 1, grid.height / 10);
		transform.position += new Vector3(0,0,grid.radius/2); 
	}

}
