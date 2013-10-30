using UnityEngine;
using System.Collections;

public class CameraToHeli : MonoBehaviour {
	
	private Transform myTransform;
	public Transform myTarget;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.LookAt(myTarget);
	}
}
