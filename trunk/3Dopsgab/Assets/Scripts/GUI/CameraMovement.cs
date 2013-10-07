using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
	
	private const int ScrollSpeed = 200;
	private const int ScrollWidth = 10;
	private const int MaxCameraHeight = 200;
	private const int MinCameraHeight = 100;
	private const int RotateAmount = 10;
	private const int RotateSpeed = 100;
	
	void Update ()
	{
		MoveCamera ();
		RotateCamera ();
	}
	
	private void MoveCamera ()
	{
		if (Camera.main != null) {
			float xpos = Input.mousePosition.x;
			float ypos = Input.mousePosition.y;
			Vector3 movement = Vector3.zero;
		
			int width = Screen.width;
			int height = Screen.height;
		
			//horizontal movement
			if ((xpos >= 0 && xpos < ScrollWidth || Input.GetKey (KeyCode.A)))
				movement.x -= ScrollSpeed;
			else if (xpos <= width && xpos > width - ScrollWidth || Input.GetKey (KeyCode.D))
				movement.x += ScrollSpeed;
		
			//vertical camera movement
			if (ypos >= 0 && ypos < ScrollWidth || Input.GetKey (KeyCode.S))
				movement.z -= ScrollSpeed;
			else if (ypos <= height && ypos > height - ScrollWidth || Input.GetKey (KeyCode.W))
				movement.z += ScrollSpeed;
		
			movement = Camera.main.transform.TransformDirection (movement);
			movement.y = 30;
		
			//away from the ground
			movement.y = ScrollSpeed * Input.GetAxis ("Mouse ScrollWheel");
		
			Vector3 origin = Camera.main.transform.position;
			Vector3 destination = origin;
			if (movement != Vector3.zero)
				destination += movement;
		
			if (destination.y > MaxCameraHeight)
				destination.y = MaxCameraHeight;
			else if (destination.y < MinCameraHeight)
				destination.y = MinCameraHeight;
		
			if (origin != destination)
				Camera.main.transform.position = Vector3.MoveTowards (origin, destination, ScrollSpeed * Time.deltaTime);
		}
	}

	private void RotateCamera ()
	{
		if (Camera.main != null) {
			Vector3 origin = Camera.main.transform.eulerAngles;
			Vector3 destination = origin;
		
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetMouseButton (0))
				destination.y += Input.GetAxis ("Mouse X") * RotateAmount;
		
			if (origin != destination)
				Camera.main.transform.eulerAngles = Vector3.MoveTowards (origin, destination, Time.deltaTime * RotateSpeed);
		}
	}
}
