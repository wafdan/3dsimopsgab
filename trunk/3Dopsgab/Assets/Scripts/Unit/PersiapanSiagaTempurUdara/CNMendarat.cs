using UnityEngine;
using System.Collections;

public class CNMendarat : MonoBehaviour {
	
	private Transform myTransform;
	public float CNSpeed;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		StartCoroutine(CNEnd());
		
		if( myTransform.name == "CN-235_animate")
		{
			animation["Propeler_roll"].wrapMode = WrapMode.Loop;
			animation.Play("Propeler_roll");	
		}
		
		if( myTransform.name == "c-130_hercules_axis")
		{
			animation["Propeler_Rolling"].wrapMode = WrapMode.Loop;
			animation.Play("Propeler_Rolling");
		}
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.forward * CNSpeed * Time.deltaTime);
	}
	
	IEnumerator CNEnd()
	{
		yield return new WaitForSeconds(19);
		if( CNSpeed != 0)
		{
			Destroy(myTransform.gameObject);
		}
		Debug.Log("Ganti Scene");
	}
}
