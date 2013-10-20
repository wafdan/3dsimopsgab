using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
	
	private Transform myTransform;
	
	public float kecepatanMissile = 35;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		StartCoroutine(DestroyMissille());
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.up * kecepatanMissile * Time.deltaTime);
		
	}
	
	IEnumerator DestroyMissille()
	{
		yield return new WaitForSeconds(4);
		Destroy(myTransform.gameObject);
	}
	
	IEnumerator HoldMissile()
	{
		yield return new WaitForSeconds(2);	
			
	}
}
