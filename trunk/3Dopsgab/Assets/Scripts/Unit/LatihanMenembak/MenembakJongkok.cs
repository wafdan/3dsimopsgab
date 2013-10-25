using UnityEngine;
using System.Collections;

public class MenembakJongkok : MonoBehaviour {
	
	private Transform myTransform;
	public Transform myBullet;
	public Transform bulletExplosion;
	
	private Transform bulletSource;
	private Vector3 bulletSourcePos = new Vector3();
	
	
	private bool startMenembak = false;
	
	private float nextFire = 0;
	private float rateFire = 0.2f;
	
	// Use this for initialization
	void Start () {
		
		myTransform = transform;
		
		animation.Play("stand");
		
		bulletSource = myTransform.FindChild("KeluarPeluru");
		
		StartCoroutine(BersiapMenembak());
		StartCoroutine(LakukanTembakan());
		StartCoroutine(SelesaiMenembak());
	}
	
	// Update is called once per frame
	void Update () {
		
		if( startMenembak == true && Time.time > nextFire && Time.time > 7f)
		{
			nextFire = Time.time + rateFire;
			
			bulletSourcePos = myTransform.TransformPoint(0.004946231f, 0.1172145f, 0.1551267f);
			
			Instantiate(bulletExplosion, bulletSourcePos, bulletSource.rotation);
			Instantiate(myBullet, bulletSourcePos, bulletSource.rotation);	
		}
	}
	
	IEnumerator BersiapMenembak()
	{
		yield return new WaitForSeconds(6f);
		animation.Play("stand_to_hip");
		Debug.Log("stand to hip");
	}
	
	IEnumerator LakukanTembakan()
	{
		yield return new WaitForSeconds(10);
		startMenembak = true;
		Debug.Log("mulaimenembak");
	}
	
	IEnumerator SelesaiMenembak()
	{
		yield return new WaitForSeconds(14);
		animation.Play("hip_to_stand");
		startMenembak = false;
		Debug.Log("berhenti menembak");
	}
}
