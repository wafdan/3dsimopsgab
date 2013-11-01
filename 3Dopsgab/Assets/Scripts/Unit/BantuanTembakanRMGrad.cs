using UnityEngine;
using System.Collections;

public class BantuanTembakanRMGrad : MonoBehaviour {
	
	private Transform myTransform;
	public Transform myMissile;
	
	public float fireRate;
	private float nextFire;
	
	private Vector3 missileSource = new Vector3();
	private Transform theMissile;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		
		theMissile = myTransform.FindChild("Source");
		
	}
	
	// Update is called once per frame
	void Update () {
		if( Time.time > nextFire && Time.time > 4 && Time.time < 7)
		{
			nextFire = Time.time + fireRate;
			
			missileSource = theMissile.position;
			
			Instantiate(myMissile, missileSource, theMissile.rotation);
		}
	}
	
	
}
