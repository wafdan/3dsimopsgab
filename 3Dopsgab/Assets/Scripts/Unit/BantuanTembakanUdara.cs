using UnityEngine;
using System.Collections;

public class BantuanTembakanUdara : MonoBehaviour {
	
	private Transform myTransform;
	public Transform myMissile;
		
	private Transform missileSourceOne;
	private Transform missileSourceTwo;
	
	private float nextFire = 0;
	private float fireRate = 0.5f;
	
	private Vector3 launchOne = new Vector3();
	private Vector3 launchTwo = new Vector3();
	
	private bool stopFiring = false;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		
		missileSourceOne = myTransform.FindChild("MissileOne");
		missileSourceTwo = myTransform.FindChild("MissileTwo");
		
		StartCoroutine(StopFiring());
	}
	
	// Update is called once per frame
	void Update () {
		
				
		if( stopFiring == false)
		{
			myTransform.Translate( Vector3.forward * 10 * Time.deltaTime);
			
			if( Time.time > nextFire && Time.time > 3f)
			{
				nextFire = fireRate + Time.time;
				
				launchOne = myTransform.TransformPoint( -0.08613683f, -0.02411515f, -0.01987852f);
				launchTwo = myTransform.TransformPoint( 0.09564326f, -0.02526893f, -0.01229101f);
							
				Instantiate(myMissile, launchOne, missileSourceOne.rotation);
				Instantiate(myMissile, launchTwo, missileSourceTwo.rotation);
			}
		}
		
		else
		{
			myTransform.Translate( Vector3.forward * 10 * Time.deltaTime);
			//myTransform.Translate( Vector3.right * 10 * Time.deltaTime);
			myTransform.Rotate(0,0,-10 * Time.deltaTime);	
			
			StartCoroutine(ChangeScene());
		}
	}
	
	IEnumerator StopFiring()
	{
		yield return new WaitForSeconds(5f);
		stopFiring = true;
	}
	
	IEnumerator ChangeScene()
	{
		yield return new WaitForSeconds(6f);	
		Debug.Log("Ganti Scene/Destroy Scene this additive");
	}
}
