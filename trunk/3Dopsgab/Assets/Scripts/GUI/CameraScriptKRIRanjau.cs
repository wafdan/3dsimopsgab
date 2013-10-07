using UnityEngine;
using System.Collections;

public class CameraScriptKRIRanjau : MonoBehaviour {
	
	private Camera myCamera;
	private Transform cameraHeadTransform;
	
	// Use this for initialization
	void Start () {
		myCamera = Camera.main;
		cameraHeadTransform = transform.FindChild("CameraHead");
	}
	
	// Update is called once per frame
	void Update () {
		myCamera.transform.position = cameraHeadTransform.position;
		myCamera.transform.rotation = cameraHeadTransform.rotation;
	}
}
