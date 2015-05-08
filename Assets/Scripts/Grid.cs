using UnityEngine;
using System;

// гексогональное игровое поле
public class Grid : MonoBehaviour {

	public float radius;
	public int countTail = 5;
	public int col_lowIndex{ get; private set; }
	public int col_highIndex{ get; private set; }
	public int row_lowIndex{ get; private set; }
	public int row_highIndex{ get; private set; }
	public float width{ get; private set; }
	public float height{ get; private set; }

	private Vector3[,] grid;

	// проверка индексов ячеек игрового поля
	private bool ok( int col, int row )
	{
		if (col >= col_lowIndex && col <= col_highIndex && row >= row_lowIndex && row <= row_highIndex)
		{
			return true;
		}

		return false;
	}

	// индексатор ячеек игрового поля
	public Vector3 this[int icol, int irow]{
		get
		{
			if (ok (icol, irow))
		 	{
				return grid [icol-col_lowIndex, irow-row_lowIndex];
		 	}

			throw new IndexOutOfRangeException();
		}
		set
		{
			if (ok (icol, irow))
			{
				grid [icol-col_lowIndex, irow-row_lowIndex] = value;
			}
		}
	}

	void Awake ()
	{
		float rowDist = 2 * radius;
		float colDist = 2*radius/Mathf.Sqrt (3f) + radius/2;
		row_highIndex = countTail;
		row_lowIndex = - row_highIndex;
		col_highIndex = countTail;
		col_lowIndex = - col_highIndex;
		grid = new Vector3[col_highIndex - col_lowIndex + 1,row_highIndex - row_lowIndex + 1];
		float offset;

		// построение координатных точек игрового поля (центры ячеек)
		for( int c = 0; c <= col_highIndex - col_lowIndex; ++c)
		{
			for( int r = 0; r <= row_highIndex - row_lowIndex; ++r)
			{
				offset = 0;
				if((c%2 != 0 && col_highIndex%2 == 0) || (c%2 == 0 && col_highIndex%2 != 0))
				{
					offset = radius;
				}
				grid[c,r] = new Vector3(colDist*(col_lowIndex + c),0.1f,rowDist*(row_lowIndex + r)+offset);
			}
		}

		// определение длинны и ширины игрового поля
		width = grid [col_highIndex - col_lowIndex, 0].x - grid [0, 0].x + 4 * radius / Mathf.Sqrt (3);
		height = (2 * countTail + 1) * 2 * radius + radius;
	}
}
