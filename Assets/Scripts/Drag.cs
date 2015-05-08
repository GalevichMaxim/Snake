using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

// перетаскивание джойстика по экрану
public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler{
	Vector3 prevMousePos;

	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		prevMousePos = Input.mousePosition;
	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		transform.parent.position += Input.mousePosition - prevMousePos;
		prevMousePos = Input.mousePosition;
	}

	#endregion
}