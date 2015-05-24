using UnityEngine;
using System;

// движение игрока
public class BaseMovementBehavior : MonoBehaviour {

	public GameObject joystic;

	private PlayerController playerController; // скрипт поведения игрока
	private JoysticController joysticController;
	private Grid grid;
	private IMovementState movementType;
	private float speed;
	Vector3 target;                           // координаты цели движения игрока
	Vector3 movement;						  // вектор команды направления движения игрока
	int[] index = {0,1};
	int[] curIndex;
	float fract;
	FirstPersonMovement firstPerson;
	Vector3 lastPos;

	void Start()
	{
		grid = GameObject.FindGameObjectWithTag("LevelController").GetComponent<Grid>();
		playerController = GetComponent<PlayerController> ();
		joysticController = joystic.GetComponent<JoysticController> (); // определяем контроллер джойстика
		movementType = new ForwardMovement ();
		curIndex = new int[2];
		target = grid [0, 1];
		movement = Vector2.zero;             // начальное положение игрока - без движения
		firstPerson = new FirstPersonMovement ();
		lastPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
	}
	
	public void FixedUpdate()
	{
		if(GameManager.Instance.Pause)       // если в игре пауса, тогда игрок не двигается
		{
			return;
		}
		try{
			speed = playerController.speed;   // установка текущей скорости движения игрока
			// проверка ввода команды от джойстика
			Vector2 joyPress = joysticController.Pressed;
			// проверка ввода команды от клавиатуры
			float h = Input.GetAxis ("Horizontal") + joyPress.x;
			float v = (Input.GetAxis ("Vertical") + joyPress.y) * firstPerson.direct.y;
			if( h != 0 || v != 0 )
			{
				movement = Vector3.zero;
				movement.x = h;
				movement.z = v;
				Input.ResetInputAxes();
			}
			if(playerController.damage)       // при фатальном столкновении
			{
				movementType.Move(gameObject, movement, out movementType);
				movement = Vector3.zero;
				return;
			}
			float fract = speed / (2 * grid.radius);
			Vector3 direct = (target - lastPos).normalized;
			lastPos += direct * fract;
			rigidbody.MovePosition (lastPos);
			if( Mathf.Abs((transform.position - target).magnitude) <= 0.2f )
			{
				lastPos = target;
				movementType.Move(gameObject, movement, out movementType);
				movementType.nextCell( index );
				target = grid [index[0], index[1]];
				curIndex[0] = index[0];          // если цель находится в пределах игрового поля,
				curIndex[1] = index[1];			 // то запоминаем её координаты как действительные
				movement = Vector3.zero;
			}
		}
		catch(IndexOutOfRangeException)          // действия при выходе игрока за границы игрового поля
		{
			index[0] = curIndex[0];
			index[1] = curIndex[1];
			playerController.health = --playerController.health < 0 ? 0 : playerController.health;
			playerController.PlayDamage();     // выход за пределы поля - фатальная ошибка для игрока и проигрывается определённая мелодия
			playerController.damage = true;
		}
	}

	public void SetMovement(IMovementState _movementType)
	{
		// установка состояния движения игрока
		movementType = _movementType;
	}
	
}
