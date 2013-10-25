using UnityEngine;
using System.Collections;

public class modelMajuPesawat : MonoBehaviour {
	
	private Transform myTransform;
	public float kecepatan = 10;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		StartCoroutine(destroyModel());
		
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
		myTransform.Translate(Vector3.forward * kecepatan * Time.deltaTime);
		
	}
	
	IEnumerator destroyModel()
	{
		yield return new WaitForSeconds(16f);
		Destroy(myTransform.gameObject);
	}
	
}
