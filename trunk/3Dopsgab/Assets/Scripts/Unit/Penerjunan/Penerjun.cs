using UnityEngine;
using System.Collections;

public class Penerjun : MonoBehaviour {
	
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		StartCoroutine(penerjunOut());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator penerjunOut()
	{
		yield return new WaitForSeconds(8);
		Destroy(myTransform.gameObject);
	}
}
