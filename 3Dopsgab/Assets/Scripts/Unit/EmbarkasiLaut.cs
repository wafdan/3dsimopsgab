using UnityEngine;
using System.Collections;

public class EmbarkasiLaut: MonoBehaviour {
	
	private Transform myTransform;
	public float kecepatanKamera;
	
	public Transform myTarget;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.LookAt(myTarget);
		
		myTransform.Translate(Vector3.forward * kecepatanKamera * Time.deltaTime);
	}
}
