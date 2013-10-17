using UnityEngine;
using System.Collections;

public class cameraPenerbanganLepasLandas : MonoBehaviour {
	
	public Transform myTarget;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(myTarget);
	}
}
