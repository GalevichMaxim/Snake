using UnityEngine;
using System.Collections;

// интерфейс состояния движения игрока
public interface IMovementState {

	void Move(GameObject entity, Vector3 input, out IMovementState nextState); // определение следующего состояния
	void nextCell (int[] currCell);											   // определение следующей ячейки
}

public class FirstPersonMovement
{
	public Vector2 direct {
		get{
			if(GameManager.Instance.curTCamera != typeCamera.TOP_VIEW)
			{
				return new Vector2(-1,0);
			}
			else
			{
				return new Vector2(1,1);
			}
		}
	}
}

// движение вперёд
public class ForwardMovement : IMovementState{

	public void Move(GameObject obj, Vector3 input, out IMovementState nextState)
	{
		nextState = this;
		float h = input.x;

		if (h > 0)
		{
			obj.transform.Rotate(0f,60f,0f);
			nextState = new RightTopMovement();
		}
		else if(h < 0)
		{
			obj.transform.Rotate(0f,-60f,0f);
			nextState = new LeftTopMovement();
		}
	}

	public void nextCell(int[] currCell)
	{
		currCell[1] += 1;
	}
	
}

// движение по диогонали вправо вверх
public class RightTopMovement : IMovementState{
	
	public void Move(GameObject obj, Vector3 input, out IMovementState nextState)
	{
		nextState = this;
		float h = input.x;
		float v = input.z;

		if (h > 0 || v < 0)
		{
			obj.transform.Rotate(0f,60f,0f);
			nextState = new RightBottomMovement();
		}
		else if(h < 0 || v > 0)
		{
			obj.transform.Rotate(0f,-60f,0f);
			nextState = new ForwardMovement();
		}
	}

	public void nextCell(int[] currCell)
	{
		if(currCell[0] % 2 == 0)
		{
			currCell[0] += 1;
		}
		else
		{
			currCell[0] += 1;
			currCell[1] += 1;
		}
	}
}

// движение по диогонали вправо вниз
public class RightBottomMovement : FirstPersonMovement, IMovementState{
	
	public void Move(GameObject obj, Vector3 input, out IMovementState nextState)
	{
		nextState = this;
		float h = input.x * direct.x;
		float v = input.z * direct.y;

		if (h > 0 || v > 0)
		{
			obj.transform.Rotate(0f,-60f,0f);
			nextState = new RightTopMovement();
		}
		else if(h < 0 || v < 0)
		{
			obj.transform.Rotate(0f,60f,0f);
			nextState = new BottomMovement();
		}
	}

	public void nextCell(int[] currCell)
	{
		if(currCell[0] % 2 == 0)
		{
			currCell[0] += 1;
			currCell[1] += -1;
		}
		else
		{
			currCell[0] += 1;
		}
	}
}

// двжение вниз
public class BottomMovement : FirstPersonMovement, IMovementState{
	
	public void Move(GameObject obj, Vector3 input, out IMovementState nextState)
	{
		nextState = this;
		float h = input.x  * direct.x;
				
		if (h > 0)
		{
			obj.transform.Rotate(0f,-60f,0f);
			nextState = new RightBottomMovement();
		}
		else if(h < 0)
		{
			obj.transform.Rotate(0f,60f,0f);
			nextState = new LeftBottomMovement();
		}
	}

	public void nextCell(int[] currCell)
	{
		currCell[1] += -1;
	}
}

// движение по диагонали влево вниз
public class LeftBottomMovement : FirstPersonMovement, IMovementState{
	
	public void Move(GameObject obj, Vector3 input, out IMovementState nextState)
	{
		nextState = this;
		float h = input.x  * direct.x;
		float v = input.z  * direct.y;
			
		if (h > 0 || v < 0)
		{
			obj.transform.Rotate(0f,-60f,0f);
			nextState = new BottomMovement();
		}
		else if(h < 0 || v > 0)
		{
			obj.transform.Rotate(0f,60f,0f);
			nextState = new LeftTopMovement();
		}
	}

	public void nextCell(int[] currCell)
	{
		if(currCell[0] % 2 == 0)
		{
			currCell[0] += -1;
			currCell[1] += -1;
		}
		else
		{
			currCell[0] += -1;
		}
	}
}

// движение по диагонали влево вверх
public class LeftTopMovement : IMovementState{
	
	public void Move(GameObject obj, Vector3 input, out IMovementState nextState)
	{
		nextState = this;
		float h = input.x;
		float v = input.z;
		if (h > 0 || v > 0)
		{
			obj.transform.Rotate(0f,60f,0f);
			nextState = new ForwardMovement();
		}
		else if(h < 0 || v < 0)
		{
			obj.transform.Rotate(0f,-60f,0f);
			nextState = new LeftBottomMovement();
		}
	}

	public void nextCell(int[] currCell)
	{
		if(currCell[0] % 2 == 0)
		{
			currCell[0] += -1;
		}
		else
		{
			currCell[0] += -1;
			currCell[1] += 1;
		}
	}
}