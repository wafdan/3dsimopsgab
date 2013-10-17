using UnityEngine;
using System.Collections;

public class parasuthilang : MonoBehaviour {
	
	public static bool parasutHilang = false;
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter( Collider otherObject)
	{
		if( otherObject.transform.tag == "daratan")
		{
			Destroy(myTransform.gameObject);
		}
	}
}
