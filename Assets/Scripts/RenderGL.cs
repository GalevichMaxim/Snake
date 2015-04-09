using UnityEngine;
using System.Collections;

public class RenderGL : MonoBehaviour {

	private GameController gameController;

	void Awake()
	{
		gameController = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<GameController> ();
	}

	void OnPostRender()
	{
		gameController.CreatePlayField ();
	}
}
