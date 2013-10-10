using UnityEngine;
using System.Collections;

public class cameraToTarget : MonoBehaviour {
	
	public Transform myTarget;
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.LookAt(myTarget);
	}
}
