using UnityEngine;
using System.Collections;

public class DragSelect : MonoBehaviour {
	
	public GameObject selector;
	private Vector3 corner;
	private GameObject selectorInstance;
	
	void OnMouseDown() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit info;
		Physics.Raycast(ray, out info, Mathf.Infinity, 1);
		
		corner = info.point;
		selectorInstance = Instantiate(selector, corner, selector.transform.rotation) as GameObject;
	}
	
	void OnMouseDrag() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//Debug.DrawRay(ray.GetPoint(10),Vector3.up*3);
		RaycastHit info;
		Physics.Raycast(ray, out info, Mathf.Infinity, 1);
		
		Vector3 resizeVector = info.point - corner;
		Vector3 newScale = selectorInstance.transform.localScale;
		newScale.x = resizeVector.x;
		newScale.z = -resizeVector.z;
		selectorInstance.transform.localScale = newScale;
	}
	
	void OnMouseUp() {
		Destroy(selectorInstance);
	}
}
