using UnityEngine;
using System.Collections;

public class komandanBarisUnitUdara : MonoBehaviour {
	
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		
		myTransform = transform;
		
		animation["walk_no_weapon"].wrapMode = WrapMode.Loop;
		animation["walk"].wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
		if( myTransform.name == "komandan")
		{
			animation.Play("walk_no_weapon");
		}
		
		if( myTransform.name == "PrajuritCN")
		{
			animation.Play("walk");	
		}
		
		myTransform.Translate(Vector3.forward * 4 * Time.deltaTime);
	}
	
	void OnTriggerEnter( Collider otherObject)
	{
		if( otherObject.tag == "pintu")
		{
			Debug.Log("kena pintu");	
			Destroy(myTransform.gameObject);
		}
	}
}
