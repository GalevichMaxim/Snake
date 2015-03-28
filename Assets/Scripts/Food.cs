using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	public int points = 10;
	
	public void Update()
	{
		transform.Rotate(Vector3.up, 60 * Time.deltaTime);
	}
	
	public void Eat()
	{

		GameController.points += points;
		
		Destroy(gameObject);

	}
}
