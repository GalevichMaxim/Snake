using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;

	private Vector3 offset;

	void Awake()
	{
	}

	void Start()
	{
		switch (GameManager.Instance.curTCamera)
		{
			case typeCamera.FIRST_PERSON:
			{
				transform.position = target.position + new Vector3(0f,2f,2f);
				transform.rotation.SetLookRotation(new Vector3(60,0,0));
				break;
			}
			case typeCamera.THIRD_PERSON:
			{
				transform.position = new Vector3(0,25,-30);
				transform.rotation.SetLookRotation(new Vector3(60,0,0));
				break;
			}
		}
		offset = transform.position - target.position;
	}
	
	public void Update()
	{
		if (target != null)
		{
			transform.position = target.position + offset;
			if( GameManager.Instance.curTCamera == typeCamera.FIRST_PERSON )
			{
				transform.rotation = target.rotation;
				transform.Rotate(Vector3.right,30);
			}
		}
	}
}
