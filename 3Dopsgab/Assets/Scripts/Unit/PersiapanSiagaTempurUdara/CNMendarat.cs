using UnityEngine;
using System.Collections;

public class CNMendarat : MonoBehaviour {
	
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		StartCoroutine(CNEnd());
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.forward * 60 * Time.deltaTime);
	}
	
	IEnumerator CNEnd()
	{
		yield return new WaitForSeconds(19);
		Destroy(myTransform.gameObject);
	}
}
