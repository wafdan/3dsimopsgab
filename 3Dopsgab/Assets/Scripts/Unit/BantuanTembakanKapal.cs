using UnityEngine;
using System.Collections;

public class BantuanTembakanKapal : MonoBehaviour {
	
	private Transform missileSource;
	private Transform myTransform;
	
	public Transform missile;
	
	private Vector3 launchPosition = new Vector3();
	
	private float fireRate = 3f;
	private float nextFire = 0;
	
	
	// Use this for initialization
	void Start () {
		
		myTransform = transform;
		missileSource = myTransform.FindChild("SumberMissile");
	}
	
	// Update is called once per frame
	void Update () {
		
		if( Time.time > nextFire && Time.time > 3.0f)
		{
			nextFire = fireRate + Time.time;
			
			launchPosition = missileSource.TransformPoint( -1.39542e-15f, 1.02718f, 0.4f);
			
			Instantiate( missile, launchPosition, missileSource.rotation);
		}
	}
	
}
