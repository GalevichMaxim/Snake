using UnityEngine;
using System.Collections;

public interface IMovementState {

	void Move(GameObject entity, Vector3 input, out IMovementState nextState);
	void nextCell (int[] currCell);
}

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

public class RightBottomMovement : IMovementState{
	
	public void Move(GameObject obj, Vector3 input, out IMovementState nextState)
	{
		nextState = this;
		float h = input.x;
		float v = input.z;

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

public class BottomMovement : IMovementState{
	
	public void Move(GameObject obj, Vector3 input, out IMovementState nextState)
	{
		nextState = this;
		float h = input.x;
				
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

public class LeftBottomMovement : IMovementState{
	
	public void Move(GameObject obj, Vector3 input, out IMovementState nextState)
	{
		nextState = this;
		float h = input.x;
		float v = input.z;
			
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