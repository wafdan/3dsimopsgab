using UnityEngine;
using System.Collections;

public class HeliMenembak : MonoBehaviour {
	
	private Transform myTransform;
	public Transform missile;
	private bool startFire = false;
		
	private float nextFire = 0;
	private float fireRate = 1f;
	
	private Transform missileSource1;
	private Transform missileSource2;
	private Vector3 missileLocation1 = new Vector3();
	private Vector3 missileLocation2 = new Vector3();
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		
		missileSource1 = myTransform.FindChild("1");
		missileSource2 = myTransform.FindChild("2");
		
		StartCoroutine(StartFire());
	}
	
	// Update is called once per frame
	void Update () {
		if( startFire == true && Time.time > nextFire && Time.time < 9)
		{
			nextFire = Time.time + fireRate;	
			
			//missileLocation1 = myTransform.TransformPoint(-0.01679062f, 0.002063217f, 0.001649779f);
			//missileLocation2 = myTransform.TransformPoint(0.01662491f, 0.002063217f, 0.001649782f);
			
			missileLocation1 = missileSource1.position;
			missileLocation2 = missileSource2.position;
			
			Instantiate( missile, missileLocation1, missileSource1.rotation);
			Instantiate( missile, missileLocation2, missileSource2.rotation);
		}
		Debug.Log(missileLocation1);
	}
	
	IEnumerator StartFire()
	{
		yield return new WaitForSeconds(5);	
		startFire = true;
	}
}
