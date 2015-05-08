using UnityEngine;
using System.Collections;

// настройка границы (правой) игрового поля
public class InitEast : MonoBehaviour {

	public Grid grid;

	void Start () 
	{
		grid = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<Grid> ();
		transform.localScale = new Vector3 (1, 2, grid.height);
		transform.position = new Vector3 (grid.width / 2, 0f, grid.radius / 2);
	}
}
