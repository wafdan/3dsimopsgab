using UnityEngine;
using System.Collections;

public class LepasLandas : MonoBehaviour {
	
	public float kecepatanPesawat;
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		
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
		float acceleration = 0;
		float rotationSpeed = 0;
		
		acceleration += 2f * Time.time;
	
		
		myTransform.Translate(Vector3.forward * kecepatanPesawat * acceleration * Time.deltaTime);	
		
		if( acceleration > 3f)
		{
			rotationSpeed -= (4*Time.deltaTime);
			
			myTransform.Rotate(rotationSpeed,0,0);
			
			StartCoroutine(destroyPlan());
		}
		
		Debug.Log(rotationSpeed);
	}
	
	IEnumerator destroyPlan()
	{
		yield return new WaitForSeconds(10f);
		Destroy(myTransform.gameObject);
	}
}
