using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public Material wallMaterial;
	public int countWals = 10;
	public static int points;
	public Text ScoreText;
	public Text HealthText;
	public GameObject apple;
	public Animator anim;

	private int _lastPonts = -1;
	private int _lastPlayerHealth = -1;
	private PlayerController playerController;
		
	public void Awake()
	{
		points = 0;
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
		GenerateLevel();
		GenerateNewFood();
	}
	
	public void Update()
	{
		if (playerController.health == 0)
		{
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

		if (points > 0)
		{ 
			GenerateNewFood ();
		}
	}


	public void GenerateNewFood()
	{
		//GameObject food = (GameObject)Instantiate(Resources.Load("Prefabs/Apple", typeof(GameObject)));
		GameObject food = Instantiate(apple) as GameObject;
		// цикл подбора положения еды
		while (true)
		{
			// ставим еду в рандомное место
			food.transform.position = new Vector3(Random.Range(-49, 49), 0, Random.Range(-49, 49));
			// получаем размер ее колайдера в мировых координатах
			Bounds foodBounds = food.collider.bounds;
			
			bool intersects = false;
			
			// Проверяем со всеми колайдерами кроме колайдера самой еды.
			// Данная фукнция использует габаритные контейнеры колайдеров для
			// сравнения. Если используются сложные колайдеры в уровне, то
			// данное сравнение будет не верным.
			Collider[] objects = FindObjectsOfType(typeof(Collider)) as Collider[];
			foreach (Collider objectColiider in objects)
			{
				if (objectColiider != food.collider && objectColiider.gameObject.tag != "Food")
				{
					// если пересекается, то завершаем цикл, досрочно
					if (objectColiider.bounds.Intersects(foodBounds))
					{
						intersects = true;
						break;
					}
				}
			}
			
			// установили в нужное место, останавливаем цикл установки
			if (!intersects)
			{
				break;
			}
		}
	}

	private void GenerateLevel()
	{
		for (int i = 0; i < countWals; i++)
		{
			GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);

			wall.name = "Wall";

			wall.transform.localScale = new Vector3(2,2,2);
			
			var pos = new Vector3(Random.Range(-49, 49), 0, Random.Range(-49, 49));
			while (Mathf.Abs(pos.x) < 10 || Mathf.Abs(pos.z) < 10)
			{
				pos = new Vector3(Random.Range(-49, 49), 0, Random.Range(-49, 49));
			}
			wall.transform.position = pos;

			wall.renderer.material = wallMaterial;
		}
		
	}
}
