using UnityEngine;
using System.Collections;

public class AmpibiDebarkasi : MonoBehaviour {
	
	private Transform myTransform;
	public float ampibiSpeed;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		
		if( myTransform.name == "perahu_karet")
		{
			StartCoroutine(DestroyThis());	
		}
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate( Vector3.forward * ampibiSpeed * Time.deltaTime);
	}
	
	IEnumerator DestroyThis()
	{
		yield return new WaitForSeconds(50);
		Destroy(myTransform.gameObject);
	}
}
