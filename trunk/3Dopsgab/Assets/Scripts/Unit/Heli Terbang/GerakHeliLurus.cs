using UnityEngine;
using System.Collections;

public class GerakHeliLurus : MonoBehaviour {
	
	public float heliSPeed = 20;
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		StartCoroutine(DestroyHeli());
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate( Vector3.forward * heliSPeed * Time.deltaTime);
	}
	
	IEnumerator DestroyHeli()
	{
		yield return new WaitForSeconds(30);
		Destroy(myTransform.gameObject);
	}
}
