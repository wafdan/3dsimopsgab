using UnityEngine;
using System.Collections;

public class komandanBarisUnitUdara : MonoBehaviour {
	
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		
		myTransform = transform;
		
		animation["walk_no_weapon"].wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
		animation.Play("walk_no_weapon");
		myTransform.Translate(Vector3.forward * 5 * Time.deltaTime);
	}
}
