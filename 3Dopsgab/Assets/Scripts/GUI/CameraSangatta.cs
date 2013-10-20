using UnityEngine;
using System.Collections;

public class CameraSangatta : MonoBehaviour
{
	
	private const int ScrollSpeed = 200;
	private const int ScrollWidth = 10;
	private const int MaxCameraHeight = 600;
	private const int MinCameraHeight = 100;
	private const int RotateAmount = 10;
	private const int RotateSpeed = 100;
	
	void start(){
	}
	
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
			
			Vector3 origin = Camera.main.transform.position;
			Vector3 destination = origin;
		
			int width = Screen.width;
			int height = Screen.height;
		
			//horizontal movement
			if ((xpos >= 0 && xpos < ScrollWidth || Input.GetKey (KeyCode.A)))
			{			
				movement.x -= ScrollSpeed;
				if( origin.x <= 1300f)
				{
					origin = new Vector3(1300f, origin.y, origin.z);	
				}
			}	
			
			else if (xpos <= width && xpos > width - ScrollWidth || Input.GetKey (KeyCode.D))
			{
				movement.x += ScrollSpeed;
				
				if( origin.x >= 3230f)
				{
					origin = new Vector3(3230f, origin.y, origin.z);	
				}
			}
		
			//vertical camera movement
			if (ypos >= 0 && ypos < ScrollWidth || Input.GetKey (KeyCode.S))
			{
				movement.z -= ScrollSpeed;
				
				if( origin.z < -1300f)
				{
					origin = new Vector3( origin.x, origin.y, -1300f);	
				}
			}
			else if (ypos <= height && ypos > height - ScrollWidth || Input.GetKey (KeyCode.W))
			{
				movement.z += ScrollSpeed;
				
				if( origin.z > 150f)
				{
					origin = new Vector3( origin.x, origin.y, 150f);	
				}
				
			}
		
			movement = Camera.main.transform.TransformDirection (movement);
			movement.y = 30;
		
			//away from the ground
			movement.y = ScrollSpeed * Input.GetAxis ("Mouse ScrollWheel");
		
			
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
	
	// Turn on the bit using an OR operation:
    public void Show() {
    Camera.main.cullingMask = 1 << LayerMask.NameToLayer("Water");
    }
     
    // Turn off the bit using an AND operation with the complement of the shifted int:
    public void Hide() {
    camera.cullingMask = ~(1 << LayerMask.NameToLayer("Water"));
    }
     
    // Toggle the bit using a XOR operation:
    //private void Toggle() {
    //camera.cullingMask ^= 1 << LayerMask.NameToLayer("SomeLayer");
    //}
}
