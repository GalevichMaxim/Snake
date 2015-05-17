using UnityEngine;
using System.Collections;

public class WallCulling : MonoBehaviour {

	private CameraController cameraController;

	void Start()
	{
		cameraController = Camera.main.GetComponent<CameraController>();
	}

	void Update () 
	{
		//если объект находится в пределах области, ограниченной плоскостями видимости камеры
		if (GeometryUtility.TestPlanesAABB(cameraController.planes, renderer.bounds))
		{
			renderer.enabled = true;   //объект рендерится
		}
		else
		{
			renderer.enabled = false;  // иначе объект не рендерится
		}
	}
	
}
