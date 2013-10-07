using UnityEngine;
using System.Collections;

public class DetectClicks : MonoBehaviour {	

	public Camera detectionCamera;
	public bool debug = true;
	private Camera _camera;
	
	void Start() {
		if(detectionCamera != null)
		{
			_camera = detectionCamera;
		}
		else
		{
			_camera = Camera.main;
		}
	}
	
	void Update() {
		Ray ray;
		RaycastHit hit;
	
		if(Input.GetMouseButtonDown(0)) {
			ray = _camera.ScreenPointToRay(Input.mousePosition); 
			
			if(Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if(debug) {
					Debug.Log("You clicked " + hit.collider.gameObject.name,hit.collider.gameObject);
				}
				hit.transform.gameObject.SendMessage("Clicked", hit.point, SendMessageOptions.DontRequireReceiver);
				//Debug.Log("KEKLIK GA SIH!!?"+hit.transform.gameObject.name);
			}			
		}
			
		if(Input.GetMouseButtonDown(1)) {
			ray = _camera.ScreenPointToRay(Input.mousePosition); 

			if(Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if(debug) {
					Debug.Log("You right clicked " + hit.collider.gameObject.name,hit.collider.gameObject);
				}
				hit.transform.gameObject.SendMessage("RightClicked", hit.point, SendMessageOptions.DontRequireReceiver);
			}			
		}		
	}
}
