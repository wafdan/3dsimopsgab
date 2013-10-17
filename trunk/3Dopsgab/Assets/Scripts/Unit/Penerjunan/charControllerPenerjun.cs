using UnityEngine;
using System.Collections;

public class charControllerPenerjun : MonoBehaviour {
	
	private Transform myTransform;
	
	private RaycastHit hit;
	private float range = 30f;
	private bool lari = false;
	private bool melayang = true;
	
	public Transform myTarget;
	private bool hasATarget = false;
	
	// Use this for initialization
	void Start () {
		myTransform =  transform;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if( melayang == true)
		{
			animation.CrossFade("Paragliding_non_stop", 0.3f);	
		}
		
		if( lari == true)
		{
			animation.CrossFade("run",0.3f);
			
			myTransform.Translate(Vector3.forward * 5 * Time.deltaTime);
			
			myTransform.LookAt(myTarget);
		}
	
		if( hasATarget == true)
		{
			//animation.CrossFade("stand_to_hip", 0.2f);
			
			//animation.Play("idle_stand");
		}
	}
	
	void OnCollisionEnter ( Collision otherObject)
	{
		
		if( otherObject.transform.tag == "daratan")
		{
			//parasuthilang.parasutHilang = true;
			//rigidbody.isKinematic = true;
			rigidbody.useGravity = true;
			
			lari = true;
			melayang = false;
		}
	}
	
	void OnTriggerEnter (Collider theTarget)
	{
		if( theTarget.transform.tag == "target")
		{
			hasATarget = true;
			
			lari = false;
		}
	}
	
}
