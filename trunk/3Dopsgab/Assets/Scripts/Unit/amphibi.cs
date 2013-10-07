using UnityEngine;
using System.Collections;

public class amphibi : MonoBehaviour {
	
	public float kecepatanAmphibi;
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.forward * kecepatanAmphibi * Time.deltaTime);
	}
}
