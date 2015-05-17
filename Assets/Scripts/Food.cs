using UnityEngine;
using System.Collections;

// поведение еды
public class Food : MonoBehaviour {

	public int points = 10;             // очки за еду
	
	public void Update()
	{
		// постоянное вращение еды
		transform.Rotate(Vector3.up, 60 * Time.deltaTime);
	}
	
	public void Eat()
	{
		// прибавление очков за еду
		GameController.points += points;

		// удаление еды
		gameObject.SetActive(false);

	}
}
