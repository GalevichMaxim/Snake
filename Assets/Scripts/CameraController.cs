using UnityEngine;
using System.Collections;

// настройка режимов камеры
public class CameraController : MonoBehaviour {

	public Transform target;                   // объект игрока
	public float smooth = 1.5f;				   // скорость движения камеры
	public Plane[] planes{ get; set; }

	private Vector3 offset;					   // смещение камеры относительно позиции игрока
	private float relCameraPosMag;             // расстояние камеры от игрока
	private Vector3 newPos;					   // новая позиция камеры
	Vector3 cameraOffset;					   // смещение камеры
	Vector3 lastPos;

	void Awake()
	{
		// добавдяем обработчик события по изменению типа камеры
		GameManager.Instance.ChangeType += onChangeCameraType;
	}

	void Start()
	{
		//инициализация положения камеры
		switch (GameManager.Instance.curTCamera)
		{
			case typeCamera.TOP_VIEW:
			{
				lastPos = new Vector3(0f,55f,-80f);
				Quaternion angle = Quaternion.Euler(40, 0, 0);
				transform.position = lastPos;
				transform.rotation = angle;
				break;
			}
			case typeCamera.THIRD_PERSON:
			{
				cameraOffset = new Vector3(0f,5f,30f);
				Quaternion angle;
				if( target == null )
				{
					lastPos = Vector3.back * Vector3.ProjectOnPlane(cameraOffset,Vector3.up).magnitude + Vector3.up * cameraOffset.y;
					angle = Quaternion.Euler(10, 0, 0);
				}
				else{
					lastPos = target.position - target.forward * Vector3.ProjectOnPlane(cameraOffset,Vector3.up).magnitude + Vector3.up * cameraOffset.y;
					angle = Quaternion.Euler(10, target.rotation.y, 0);
				}
				angle = Quaternion.Euler(10, target.rotation.y, 0);
				transform.position = lastPos;
				transform.rotation = angle;
				break;
			}
			case typeCamera.FIRST_PERSON:
			{
				Quaternion angle;
				offset = new Vector3(0f,1.2f,0f);
				if( target == null )
				{
					lastPos = offset;
					angle = target.rotation;
				}
				else{
					lastPos = offset + target.position;
					angle = Quaternion.Euler(0,0,0);
				}
				transform.position = lastPos;
				transform.rotation = angle;
				break;
			}
		}
		planes = GeometryUtility.CalculateFrustumPlanes (camera);
	}
	
	public void FixedUpdate()
	{
		if(target == null)
		{
			return;
		}

		if (target && GameManager.Instance.curTCamera == typeCamera.THIRD_PERSON)
		{
			Vector3 newPos = target.position - target.forward * Vector3.ProjectOnPlane(cameraOffset,Vector3.up).magnitude + Vector3.up * cameraOffset.y;

			lastPos = Vector3.Lerp(lastPos, newPos, smooth / 2f * GameManager.dt);
			transform.position = lastPos;

			// поворот камеры в направлении игрока
			SmoothLookAt();
		}

		if (target && GameManager.Instance.curTCamera == typeCamera.FIRST_PERSON)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, smooth * GameManager.dt);
			transform.position = target.position + offset;
		}
	}

	void SmoothLookAt ()
	{
		Vector3 relPlayerPosition = target.position - lastPos;
		Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition);
		transform.rotation = lookAtRotation;
	}
	
	void onChangeCameraType()
	{
		Start ();
	}

	void LateUpdate()
	{
		//определяем ограничивающие плоскости камеры
		planes = GeometryUtility.CalculateFrustumPlanes (camera);
	}
}
