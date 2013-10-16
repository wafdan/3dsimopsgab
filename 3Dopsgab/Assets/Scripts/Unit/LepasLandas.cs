﻿using UnityEngine;
using System.Collections;

public class LepasLandas : MonoBehaviour {
	
	public float kecepatanPesawat;
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		float acceleration = 0;
		float rotationSpeed = 0;
		
		acceleration += 2f * Time.time;
	
		
		myTransform.Translate(Vector3.forward * kecepatanPesawat * acceleration * Time.deltaTime);	
		
		if( acceleration > 5f)
		{
			rotationSpeed -= (4*Time.deltaTime);
			
			myTransform.Rotate(rotationSpeed,0,0);
			
			StartCoroutine(destroyPlan());
		}
		
	}
	
	IEnumerator destroyPlan()
	{
		yield return new WaitForSeconds(8f);
		Destroy(myTransform.gameObject);
	}
}