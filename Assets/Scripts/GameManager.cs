using UnityEngine;
using System.Collections;
using System.IO;

public enum typeCamera {TOP_VIEW, THIRD_PERSON, FIRST_PERSON}; // типы поведения игровой камеры
public delegate void EventCamera();              // делегат для событий изменения поведения камеры

// менеджер игры
public class GameManager : MonoBehaviour {

	public event EventCamera ChangeType;           // событие изменения типа камеры
	public typeCamera curTCamera { get; set; }
	public bool gameOver = false;
	public GameObject tableScore;
	public GameObject panel;
	public int lifeCount{ get; set; }
	public Record curPlayer = null;
	public bool Pause { get; set; }

	public static float dt;

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
		}
	}

	void FixedUpdate()
	{
		// кеширование
		dt = Time.fixedDeltaTime;
	}

	void Update()
	{
		if (gameOver) 
		{
			time += Time.deltaTime;
			if (time >= restartTime) 
			{
				time = 0;
				if(curPlayer == null)
				{
					Instantiate(panel);
				}
				else
				{
					UpdateSaveScore();
					Instantiate(tableScore);
				}
				gameOver = false;
				curPlayer = null;
			}
		}
	}

	// добавление результата в таблицу рейтинга
	public void AddRecordToTableScore( string name )
	{
		BinaryWriter dataOut;
		dataOut = new BinaryWriter(new FileStream(Application.persistentDataPath + "/score.dat", FileMode.OpenOrCreate | FileMode.Append));

		dataOut.Write (name);
		dataOut.Write (GameController.points);
		dataOut.Write (lifeCount);

		dataOut.Close ();

		curPlayer = new Record (name, GameController.points, lifeCount);
	}

	// обновление рейтинга игрока
	public void UpdateSaveScore()
	{
		bool update = false;
		BinaryReader dataIn;
		BinaryWriter dataOut;
		string name;
		int score;
		int life;
		ArrayList table = new ArrayList ();
		
		dataIn = new BinaryReader(new FileStream(Application.persistentDataPath + "/score.dat", FileMode.Open));
		try{
			for(;;)
			{
				name = dataIn.ReadString();
				score = dataIn.ReadInt32();
				life = dataIn.ReadInt32();
				if(!update && name == curPlayer.name)
				{
					score = GameController.points;
					curPlayer.score = score;
					life = lifeCount;
					curPlayer.life = life;
					update = true;
				}
				table.Add( new Record( name, score, life ));
			}
		}catch(EndOfStreamException)
		{
			dataIn.Close();
		}

		dataOut = new BinaryWriter(new FileStream(Application.persistentDataPath + "/score.dat", FileMode.Create));

		foreach(Record r in table)
		{
			dataOut.Write (r.name);
			dataOut.Write (r.score);
			dataOut.Write (r.life);
		}
		
		dataOut.Close ();
	}

	public void BackToMainMenu()
	{
		Application.LoadLevelAdditive ("MainMenu");
	}
	
	public void onEventChangeCameraType()
	{
		if(ChangeType != null)
		{
			ChangeType();
		}
	}

}