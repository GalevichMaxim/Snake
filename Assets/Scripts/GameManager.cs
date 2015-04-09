using UnityEngine;
using System.Collections;

public enum typeCamera {TOP_VIEW, THIRD_PERSON};

public class GameManager : MonoBehaviour {

	public typeCamera curTCamera { get; set; }
	public bool gameOver = false;

	private static GameManager instance = null;
	private float restartTime = 3.2f;
	private float time = 0f;

	public static GameManager Instance
	{
		get {
			if (instance == null) 
			{
				instance = GameObject.FindObjectOfType<GameManager>();
				DontDestroyOnLoad (instance.gameObject);
			}
			return instance;
		}
	}

	void Awake()
	{
		if (instance)
		{
			if(this != instance)
			{
				Destroy (this.gameObject);
			}
			instance.gameOver = false;
		}
		else 
		{
			instance = this;
			DontDestroyOnLoad (this);
			curTCamera = typeCamera.TOP_VIEW; 
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
				Application.LoadLevel ("MainMenu");
			}
		}
	}
	


}