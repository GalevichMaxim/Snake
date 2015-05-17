using UnityEngine;
using System.Collections;

public class TailController : MonoBehaviour {

	public Transform target;
	public float targetDistance;
	
	
	public void Update()
	{
		if( target )
		{
			Vector3 direction = target.position - transform.position;

			if( target.gameObject.name == "Head" )
			{
				direction -= Vector3.up * 0.5f;
			}

			float distance = direction.magnitude;
			
			if (distance > targetDistance)
			{
				transform.position += direction.normalized * (distance - targetDistance);

				transform.LookAt(target);

				transform.Rotate(-transform.localEulerAngles.x,0,0);
			}
		}
	}
}
