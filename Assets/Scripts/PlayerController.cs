using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// контроллер игрока
public class PlayerController : MonoBehaviour {

	public float speed;
	public int health;
	public bool damage;
	public GameObject tailPrefab;
	public AudioClip audioDamage;
	public AudioClip audioAddScore;
	public int tailLength{ get; set; }

	private bool touch;
	private Transform current;
	private float radiusTail;
			
	public void Start()
	{
		// если не задано количество жизней, то по умолчанию устаналивается 3
		health = health == 0 ? 3 : health;
		current = transform;
		radiusTail = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<Grid> ().radius;
	}
	
	void FixedUpdate()
	{
		if( damage )
		{
			// проверка при фатальном столкневении безопасного направления движения
			if (!Physics.Raycast(transform.position, transform.forward, 2*radiusTail))
			{
				damage = false; // продолжается движение в безопасном направлении
			}
		}
	} 

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Food")
		{
			Food food = other.gameObject.GetComponent<Food> ();
			food.Eat ();
			PlayAddScore();
			AddTail ();
			speed += 0.05f;
		}
		else if(other.gameObject.tag != "Helper")
		{
			health = --health < 0 ? 0 : health;
			PlayDamage();
			damage = true;
		}
	}

	// добавление секции хвоста
	public void AddTail()
	{
		GameObject tail = Instantiate (tailPrefab) as GameObject;
		tail.transform.position = current.position - current.forward * 2;
		if( tailLength == 0 )
		{
			tail.transform.position -= Vector3.up * 0.5f;
		}
		tail.transform.rotation = transform.rotation;
		TailController tailController = tail.GetComponentInChildren<TailController> ();
		tailController.target = current;
		tailController.targetDistance = 2; 
		current = tail.transform;
		tailLength++;
	}

	public void PlayDamage()
	{
		audio.PlayOneShot (audioDamage);
	}

	public void PlayAddScore()
	{
		audio.PlayOneShot (audioAddScore);
	}
}
