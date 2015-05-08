using UnityEngine;
using System.Collections;

public class JoysticController : MonoBehaviour {

	public Transform player;

	public Vector2 Pressed{
		get{
			Vector2 res = new Vector2(h,v);
			h = 0;
			v = 0;
			return res;
		}
		private set{
			h = value.x;
			v = value.y;
		}
	}

	private float h,v;

	void Awake()
	{
		// добавдяем обработчик события по изменению типа камеры
		GameManager.Instance.ChangeType += onChangeCameraType1;
	}

	void Start()
	{
		if(GameManager.Instance.curTCamera != typeCamera.TOP_VIEW)
		{
			transform.FindChild("ButtonUp").gameObject.SetActive(false);
			transform.FindChild("ButtonDown").gameObject.SetActive(false);
		}
		else
		{
			transform.FindChild("ButtonUp").gameObject.SetActive(true);
			transform.FindChild("ButtonDown").gameObject.SetActive(true);
		}

	}

	public void OnUp()
	{
		Pressed = new Vector2 (0, 0);
		v = 1;
	}

	public void OnDown()
	{
		Pressed = new Vector2 (0, 0);
		v = -1;
	}

	public void OnRight()
	{
		Pressed = new Vector2 (0, 0);
		h = 1;
	}

	public void OnLeft()
	{
		Pressed = new Vector2 (0, 0);
		h = -1;
	}

	void onChangeCameraType1()
	{
		Start ();
	}
}
