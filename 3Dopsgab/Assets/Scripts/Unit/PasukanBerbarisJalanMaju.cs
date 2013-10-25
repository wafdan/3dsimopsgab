using UnityEngine;
using System.Collections;

public class PasukanBerbarisJalanMaju : MonoBehaviour {
	
	private Transform myTransform;
	public float walkSpeed;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		animation["walk"].wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
	}
	
	void OnTriggerEnter( Collider otherObject)
	{
		if( otherObject.transform.tag == "pintu")
		{
			StartCoroutine(InfantryDestroy());	
		}
	}
	
	IEnumerator InfantryDestroy()
	{
		yield return new WaitForSeconds(1);
		Destroy(myTransform.gameObject);
	}
}
