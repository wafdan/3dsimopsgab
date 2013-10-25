using UnityEngine;
using System.Collections;

public class MovementLaut : MonoBehaviour {
	
	private Transform myTransform;
	
	public float speed;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		
		if( myTransform.name != "Main Camera")
			StartCoroutine(Destroy());
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
	
	IEnumerator Destroy()
	{
		yield return new WaitForSeconds(11.5f);
		Destroy(myTransform.gameObject);
		Debug.Log("Ganti Scene");
	}
}
