using UnityEngine;
using System.Collections;

public class cameraPenerjunanUnit : MonoBehaviour {
	
	public Transform myTarget;
	private Transform myTransform;
	private bool stopMoving = false;
	
	//private Transform targetCamera;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		
		//targetCamera = transform.Find("Penerjun");
	}
	
	// Update is called once per frame
	void Update () {
		//myTransform.LookAt(targetCamera);
		if( stopMoving == false)
		{
			myTransform.Translate(Vector3.down * 1 * Time.deltaTime);
		}
		else
		{
			myTransform.Translate(Vector3.right * 1 * Time.deltaTime);
			myTransform.Translate(Vector3.up * 1 * Time.deltaTime);
		}
	}
	
	void OnTriggerEnter( Collider otherObject)
	{
		if( otherObject.transform.tag == "daratan")
		{
			stopMoving = true;
		}	
	}
}
