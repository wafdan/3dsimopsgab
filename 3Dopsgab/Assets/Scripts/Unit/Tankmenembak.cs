using UnityEngine;
using System.Collections;

public class Tankmenembak : MonoBehaviour {
	
	private Transform myTransform;
	public Transform theExplosion;
	
	private float nextFire = 0;
	public float fireRate;
	
	private Vector3 explosionSource = new Vector3();
	private Transform theSource;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		
		theSource = myTransform.FindChild("Source");
	}
	
	// Update is called once per frame
	void Update () {
	
		if( Time.time > nextFire && Time.time > 3)
		{
			nextFire = Time.time + fireRate;	
			
			explosionSource = theSource.position;
			
			Instantiate( theExplosion, explosionSource, theSource.rotation);
		}
	}
}
