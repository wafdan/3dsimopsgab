using UnityEngine;
using System.Collections;

public class CameraLedakanCampTank : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(GantiScene());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator GantiScene()
	{
		yield return new WaitForSeconds(8f);
		Debug.Log("gantiScene");
	}
}
