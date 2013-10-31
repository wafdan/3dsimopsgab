using UnityEngine;
using System.Collections;

public class DetonatorTank : MonoBehaviour {
	
	private Transform myTransform;
	public Transform theExplosion;
	
	private float nextExplosion = 0;
	public float explosionRate;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		if( Time.time > nextExplosion && Time.time > 3 && Time.time < 60)
		{
			nextExplosion = Time.time + explosionRate;
			
			Instantiate( theExplosion, myTransform.position, Quaternion.identity);
			Debug.Log("yes");
		}
		
	}
}
