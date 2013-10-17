using UnityEngine;
using System.Collections;

public class cameraBerbarisUnitUdara : MonoBehaviour {
	
	public Transform myTarget;
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.LookAt(myTarget);
		
		myTransform.Translate(Vector3.left * 1f * Time.deltaTime);
		myTransform.Translate(Vector3.up * 0.5f * Time.deltaTime);
	}
}
