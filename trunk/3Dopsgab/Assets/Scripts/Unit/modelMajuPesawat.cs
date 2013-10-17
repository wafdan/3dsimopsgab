using UnityEngine;
using System.Collections;

public class modelMajuPesawat : MonoBehaviour {
	
	private Transform myTransform;
	public float kecepatan = 10;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		StartCoroutine(destroyModel());
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.forward * kecepatan * Time.deltaTime);
	}
	
	IEnumerator destroyModel()
	{
		yield return new WaitForSeconds(8f);
		Destroy(myTransform.gameObject);
	}
}
