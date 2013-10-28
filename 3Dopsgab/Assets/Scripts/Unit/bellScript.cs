using UnityEngine;
using System.Collections;

public class bellScript : MonoBehaviour {
	
	private Transform myTransform;
	public float bellSpeed;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.forward * bellSpeed * Time.deltaTime);
	}
}
