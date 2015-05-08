using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// управление игровой сценной
public class GameController : MonoBehaviour {

	public Material wallMaterial;
	public Material gridMaterial;
	public Material helperMaterial;
	public static int points;
	public Text ScoreText;
	public Text HealthText;
	public Text TailText;
	public GameObject apple;
	public Animator anim;
	public GameObject wallPrefab;
	public Grid field;
	public AudioClip audioGameOver;

	private int _lastPonts = -1;
	private int _lastPlayerHealth = -1;
	private PlayerController playerController;
	private GameObject player;
	private GameObject tableScore;
	private int countWals;

	public void Start()
	{
		points = 0;
		player = GameObject.FindGameObjectWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
		field = GetComponent<Grid> ();
		countWals = field.countTail * 3;							// количество преград на поле
		GenerateLevel();											// создание преград и помощников
		GenerateNewFood();											// создание еды
	}
	
	public void Update()
	{
		// если потрачены все жизни
		if (player && playerController.health == 0 && !GameManager.Instance.gameOver)
		{
			// удаляется голова змеи ( как символ смерти)
			DestroyObject(player);
			// останов музыки игры
			audio.Stop();
			// проигрывание мелодии окончания игры
			audio.PlayOneShot(audioGameOver);
			// запуск анимации окончания игры
			anim.SetTrigger("GameOver");
			GameManager.Instance.lifeCount = playerController.health;
			GameManager.Instance.gameOver = true;
		}
		// если потрачена жизнь
		if (_lastPlayerHealth != playerController.health) 
		{
			_lastPlayerHealth = playerController.health;
			// изменение индикатора количества оставшихся жизней
			HealthText.text = _lastPlayerHealth.ToString();
		}
		if (_lastPonts == points) return;
		// если съедена еда,то меняется индикация очков и длины змейки
		_lastPonts = points;
		ScoreText.text = "Score: " + points.ToString ("0000");
		TailText.text = "Tail length: " + playerController.tailLength.ToString ("00");
		// генерируется новая еда
		if (points > 0)
		{ 
			GenerateNewFood ();
		}
	}


	public void GenerateNewFood()
	{
		GameObject food = Instantiate(apple) as GameObject;

		while (true)
		{
			Vector3 pos = field[Random.Range(field.col_lowIndex, field.col_highIndex),Random.Range(field.row_lowIndex, field.row_highIndex)];

			food.transform.position = pos;

			Bounds foodBounds = food.collider.bounds;
			
			bool intersects = false;
			
			Collider[] objects = FindObjectsOfType(typeof(Collider)) as Collider[]; // массив объектов, имеющих коллайдер
			// проверяется - не пересекается ли коллайдер еды с коллайдер других объектов 
			foreach (Collider objectColiider in objects)
			{
				if (objectColiider != food.collider && objectColiider.gameObject.tag != "Food")
				{
					if (objectColiider.bounds.Intersects(foodBounds))
					{
						// в случае пересечения цикл повторяется и определяется новая позиция еды
						intersects = true;
						break;
					}
				}
			}
			
			if (!intersects)
			{
				break;
			}
		}
	}

	void GenerateLevel()
	{
		int countHelper = countWals / 3; // 1/3 преград - это хелперы
		for (int i = 0; i < countWals; i++)
		{
			GameObject wall = Instantiate(wallPrefab) as GameObject;
			if( i < countHelper )
			{
				// для хелпера
				wall.renderer.material = helperMaterial;
				wall.tag = "Helper";                          
				wall.layer = 2;
			}

			Vector3 pos = field[Random.Range(field.col_lowIndex, field.col_highIndex),Random.Range(field.row_lowIndex, field.row_highIndex)];
			while (Mathf.Abs(pos.x) < 10 || Mathf.Abs(pos.z) < 10 )
			{
				pos = field[Random.Range(field.col_lowIndex, field.col_highIndex),Random.Range(field.row_lowIndex, field.row_highIndex)];
			}
			wall.transform.position = pos;
			 
		}
	}

	// рисование гексагональных тайлов
	public void CreatePlayField()
	{
		float r = field.radius;
		float R = 2 * field.radius / Mathf.Sqrt (3);
		GL.PushMatrix ();
		gridMaterial.SetPass (0);

		GL.Begin (GL.LINES);
		for( int ic = field.col_lowIndex; ic <= field.col_highIndex; ic++ )
			for(int ir = field.row_lowIndex; ir <= field.row_highIndex; ir++ )
			{
				GL.Vertex(field[ic, ir] + new Vector3(-r/2,-1,r));
				GL.Vertex(field[ic, ir] + new Vector3(r/2,-1,r));

				GL.Vertex(field[ic, ir] + new Vector3(r/2,-1,r));
				GL.Vertex(field[ic, ir] + new Vector3(R,-1,0));

				GL.Vertex(field[ic, ir] + new Vector3(R,-1,0));
				GL.Vertex(field[ic, ir] + new Vector3(r/2,-1,-r));

				GL.Vertex(field[ic, ir] + new Vector3(r/2,-1,-r));
				GL.Vertex(field[ic, ir] + new Vector3(-r/2,-1,-r));

				GL.Vertex(field[ic, ir] + new Vector3(-r/2,-1,-r));
				GL.Vertex(field[ic, ir] + new Vector3(-R,-1,0));

				GL.Vertex(field[ic, ir] + new Vector3(-R,-1,0));
				GL.Vertex(field[ic, ir] + new Vector3(-r/2,-1,r));
			}

		GL.End ();

		GL.PopMatrix ();

	}

	public void OnExitBtn()
	{
		tableScore = GameObject.FindGameObjectWithTag ("TableScore");
		if(tableScore)
		{
			Destroy (tableScore);
			GameManager.Instance.BackToMainMenu();
			GameManager.Instance.curPlayer = null;
			return;
		}
		if(GameManager.Instance.curPlayer != null)
		{
			Application.LoadLevelAdditive("MainMenu");
		}
		GameManager.Instance.Pause = true;
		GameManager.Instance.lifeCount = playerController.health;
		Instantiate (GameManager.Instance.panel);
	}
}
