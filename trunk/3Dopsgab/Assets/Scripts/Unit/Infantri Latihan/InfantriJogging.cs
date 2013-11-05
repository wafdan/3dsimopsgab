using UnityEngine;
using System.Collections;

public class InfantriJogging : MonoBehaviour {
	
	private Transform myTransform;
	public float joggingSpeed;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.forward * joggingSpeed * Time.deltaTime);
	}
}
