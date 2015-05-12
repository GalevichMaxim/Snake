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
	private TableScroleController tableController;

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
		curPlayer = new Record (name, GameController.points, lifeCount);

		if(Application.platform != RuntimePlatform.WindowsWebPlayer && Application.platform != RuntimePlatform.OSXWebPlayer)
		{
			BinaryWriter dataOut;
			dataOut = new BinaryWriter(new FileStream(Application.persistentDataPath + "/score.dat", FileMode.OpenOrCreate | FileMode.Append));

			dataOut.Write (name);
			dataOut.Write (GameController.points);
			dataOut.Write (lifeCount);

			dataOut.Close ();
			return;
		}

		// для WebPlayer
		string table;
		if(PlayerPrefs.HasKey("TableScore"))
		{
			table = PlayerPrefs.GetString("TableScore");
			table += string.Format(",{0},{1},{2}",name,GameController.points,lifeCount);
		}
		else
		{
			table = string.Format("{0},{1},{2}",name,GameController.points,lifeCount);
		}
		PlayerPrefs.SetString("TableScore",table);
		PlayerPrefs.Save ();
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

		if(Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
		{
			table = ParseRecords();
			string modifyTable = ",";
			foreach(Record rec in table)
			{
				if(!update && rec.name == curPlayer.name && rec.score == curPlayer.score && rec.life == curPlayer.life)
				{
					rec.score = GameController.points;
					curPlayer.score = rec.score;
					rec.life = lifeCount;
					curPlayer.life = rec.life;
					update = true;
				}
				modifyTable += string.Format("{0},{1},{2}",rec.name,rec.score,rec.life);
			}
			PlayerPrefs.SetString("TableScore",modifyTable);
			PlayerPrefs.Save();
			return;
		}

		dataIn = new BinaryReader(new FileStream(Application.persistentDataPath + "/score.dat", FileMode.Open));
		try{
			for(;;)
			{
				name = dataIn.ReadString();
				score = dataIn.ReadInt32();
				life = dataIn.ReadInt32();
				if(!update && name == curPlayer.name && score == curPlayer.score && life == curPlayer.life)
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

	public ArrayList ParseRecords()
	{
		ArrayList tableRec = new ArrayList ();
		string name = string.Empty;
		int score = 0;
		int life = 0;
		
		string records = PlayerPrefs.GetString ("TableScore");
		int index = 0;
		int first = 0;
		for( int i = 0; i <= records.Length; ++i )
		{
			if(i == records.Length || records[i] == ',')
			{
				switch(index)
				{
				case 0:
					name = records.Substring(first,i-first);
					break;
					
				case 1:
					score = int.Parse(records.Substring(first,i-first));
					break;
					
				case 2:
					life = int.Parse(records.Substring(first,i-first));
					break;
				}
				if( index == 2 )
				{
					tableRec.Add( new Record( name, score, life ));
				}
				first = i + 1;
				index = (++index) % 3; 
			}
		}
		return tableRec;
	}
}