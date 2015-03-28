using UnityEngine;
using System.Collections;

public enum typeCamera {FIRST_PERSON, THIRD_PERSON};

public class GameManager : MonoBehaviour {

	public typeCamera curTCamera { get; set; }
	public bool gameOver = false;

	private static GameManager instance = null;
	private float restartTime = 5f;
	private float time;

	public static GameManager Instance
	{
		get {
			if (instance == null) 
			{
				instance = FindObjectOfType (typeof(GameManager)) as GameManager;
			}
			return instance;
		}
	}

	void Awake()
	{
		if (instance)
		{
			Destroy (this.gameObject);
		}
		if (Instance) 
		{
			DontDestroyOnLoad (this);
			curTCamera = typeCamera.FIRST_PERSON; 
		}
	}

	void Update()
	{
		if (gameOver) 
		{
			time += Time.deltaTime;
			if (time >= restartTime) 
			{
				time = 0;
				gameOver = false;
				Application.LoadLevel ("MainMenu");
			}
		}
	}
}
