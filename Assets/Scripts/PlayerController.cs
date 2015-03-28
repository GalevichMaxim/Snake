using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 20;
	public float rotationSpeed = 60;
	public Animator anim;

	private bool touch = false;
	private Transform current;
	
	public void Start()
	{
		current = transform;
	}
	
	void FixedUpdate()
	{
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");

		Vector3 movement = new Vector3 (horizontal, 0f, vertical);
		if (horizontal != 0f || vertical != 0f) 
		{
			movement *= speed * Time.deltaTime;
			transform.rotation = Quaternion.LookRotation (movement);
		}
		touch = true;
		transform.position = transform.position + movement;
	}

	void OnTriggerEnter(Collider other)
	{
		if (touch) 
		{
			if (other.gameObject.tag == "Food")
			{
				Food food = other.gameObject.GetComponent<Food> ();
				food.Eat ();
				AddTail ();
			}
			else 
			{
				anim.SetTrigger("GameOver");
				GameManager.Instance.gameOver = true;
			}
			touch = false;
		}
	}

	public void AddTail()
	{
		TailController tail = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<TailController>();
		tail.name = "Tail";
		tail.transform.position = current.position - current.forward * 2;
		tail.transform.rotation = transform.rotation;
		tail.target = current;
		tail.targetDistance = 2;
		current = tail.transform;
	}
}
