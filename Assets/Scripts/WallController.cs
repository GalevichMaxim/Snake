using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {

	void FixedUpdate ()
	{
		if(GameManager.Instance.Pause)
		{
			rigidbody.Sleep();
		}
	}
}
