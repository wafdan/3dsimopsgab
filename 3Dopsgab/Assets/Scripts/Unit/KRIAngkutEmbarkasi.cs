using UnityEngine;
using System.Collections;

public class KRIAngkutEmbarkasi : MonoBehaviour {
	
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		animation["door_opening"].wrapMode = WrapMode.Once;
		animation["door_closing"].wrapMode = WrapMode.Once;
		
		if( myTransform.tag == "KRIMuat")
		{			
			animation.Play("door_opening");
		}
		
		if( myTransform.tag == "KRIBerangkat")
		{
			animation.playAutomatically = true;
			animation.Play("door_closing");	
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if( myTransform.tag == "KRIBerangkat")
		{
			myTransform.Translate( Vector3.forward * 5 * Time.deltaTime);
		}
	}
}
