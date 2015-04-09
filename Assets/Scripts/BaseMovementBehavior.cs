using UnityEngine;
using System;

public class BaseMovementBehavior : MonoBehaviour {

	private PlayerController playerController;
	private Grid grid;
	private IMovementState movementType;
	private float speed;
	Vector3 target;
	Vector3 movement;
	int[] index;
	int[] curIndex;
	float fract;

	void Start()
	{
		grid = GameObject.FindGameObjectWithTag("LevelController").GetComponent<Grid>();
		playerController = GetComponent<PlayerController> ();
		movementType = new ForwardMovement ();
		index = new int[2];
		curIndex = new int[2];
		target = grid [0, 1];
		movement = Vector2.zero;
	}
	
	public void FixedUpdate()
	{
		try{
			speed = playerController.speed;
			float h = Input.GetAxis ("Horizontal");
			float v = Input.GetAxis ("Vertical");
			if( h != 0 || v != 0 )
			{
				movement = Vector3.zero;
				movement.x = h;
				movement.z = v;
			}
			if(playerController.damage)
			{
				movementType.Move(gameObject, movement, out movementType);
				Input.ResetInputAxes();
				movement = Vector3.zero;
				return;
			}
			float fract = speed / (2 * grid.radius);
			Vector3 direct = (target - transform.position).normalized;
			rigidbody.MovePosition (transform.position + direct * fract);
			if( Mathf.Abs((transform.position - target).magnitude) <= 0.2f )
			{
				transform.position = target;
				movementType.Move(gameObject, movement, out movementType);
				movementType.nextCell( index );
				target = grid [index[0], index[1]];
				curIndex[0] = index[0];
				curIndex[1] = index[1];
				Input.ResetInputAxes();
				movement = Vector3.zero;
			}
		}
		catch
		{
			index[0] = curIndex[0];
			index[1] = curIndex[1];
			playerController.health = --playerController.health < 0 ? 0 : playerController.health;
			playerController.PlayDamage();
			playerController.damage = true;
		}
	}
	
	public void SetMovement(IMovementState _movementType)
	{
		movementType = _movementType;
	}
}
