using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;

	private Vector3 offset;

	void Start()
	{
		switch (GameManager.Instance.curTCamera)
		{
			case typeCamera.TOP_VIEW:
			{
				transform.position = new Vector3(0f,55f,-80f);
				Quaternion target = Quaternion.Euler(40,0,0);
				transform.rotation = target;
				offset = Vector3.zero;
				break;
			}
			case typeCamera.THIRD_PERSON:
			{
				transform.position = new Vector3(0,25,-30);
				transform.rotation.SetLookRotation(new Vector3(60,0,0));
				offset = transform.position - target.position;
				break;
			}
		}
	}
	
	public void Update()
	{
		if (target && GameManager.Instance.curTCamera == typeCamera.THIRD_PERSON)
		{
			transform.position = target.position + offset;
		}
	}
}
