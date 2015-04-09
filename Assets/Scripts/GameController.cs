using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public Material wallMaterial;
	public Material gridMaterial;
	public Material helperMaterial;
	public int countWals;
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

	public void Start()
	{
		points = 0;
		countWals = 15;
		player = GameObject.FindGameObjectWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
		field = GetComponent<Grid> ();
		GenerateLevel();
		GenerateNewFood();
	}
	
	public void Update()
	{
		if (playerController.health == 0 && !GameManager.Instance.gameOver)
		{
			DestroyObject(player);
			audio.Stop();
			audio.PlayOneShot(audioGameOver);
			anim.SetTrigger("GameOver");
			GameManager.Instance.gameOver = true;
		}
		if (_lastPlayerHealth != playerController.health) 
		{
			_lastPlayerHealth = playerController.health;
			HealthText.text = _lastPlayerHealth.ToString();
		}
		if (_lastPonts == points) return;
		_lastPonts = points;
		ScoreText.text = "Score: " + points.ToString ("0000");
		TailText.text = "Tail length: " + playerController.tailLength.ToString ("00");

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
			
			Collider[] objects = FindObjectsOfType(typeof(Collider)) as Collider[];
			foreach (Collider objectColiider in objects)
			{
				if (objectColiider != food.collider && objectColiider.gameObject.tag != "Food")
				{
					if (objectColiider.bounds.Intersects(foodBounds))
					{
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
		int countHelper = countWals / 3;
		for (int i = 0; i < countWals; i++)
		{
			GameObject wall = Instantiate(wallPrefab) as GameObject;
			if( i < countHelper )
			{
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
		Application.LoadLevel ("MainMenu");
	}
}
