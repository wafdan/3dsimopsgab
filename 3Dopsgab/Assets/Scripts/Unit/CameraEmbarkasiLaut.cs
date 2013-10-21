using UnityEngine;
using System.Collections;

public class CameraEmbarkasiLaut: MonoBehaviour {
	
	private Transform myTransform;
	public float kecepatanKamera;
	
	public Transform myTarget;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		StartCoroutine(GantiScene());
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.forward * kecepatanKamera * Time.deltaTime);
		
		//myTransform.LookAt(myTarget);
	}
	
	IEnumerator GantiScene()
	{
		yield return new WaitForSeconds(20f);
		Debug.Log("Ganti Scene");
	}
}
