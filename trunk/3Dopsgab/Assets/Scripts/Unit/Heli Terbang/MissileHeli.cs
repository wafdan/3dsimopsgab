using UnityEngine;
using System.Collections;

public class MissileHeli : MonoBehaviour {
private Transform myTransform;
	
	private float kecepatanMissile = 125;
	//public Transform asapLedakan;
	public Transform ledakan;
	
	private RaycastHit hit;
	private float range = 0.5f;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		StartCoroutine(DestroyMissille());
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.up * kecepatanMissile * Time.deltaTime);
		
		if( Physics.Raycast(myTransform.position, Vector3.up, out hit, range))
		{
			Debug.Log("darataaaaaann");
			//Instantiate(asapLedakan, myTransform.position, Quaternion.identity);
			Instantiate(ledakan, myTransform.position, Quaternion.identity);
		}
		
	}
	
	IEnumerator DestroyMissille()
	{
		yield return new WaitForSeconds(10f);
		Destroy(myTransform.gameObject);
	}
	
	IEnumerator HoldMissile()
	{
		yield return new WaitForSeconds(2);	
			
	}
}
