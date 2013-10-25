using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {
	
	private Transform myTransform;
	public AnimationClip myClip;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter( Collider otherObject)
	{		
		if( otherObject.transform.tag == "target")
		{
			Debug.Log("Door Open");	
			animation.Play("pintuCNBuka");
		}
	}	
}
