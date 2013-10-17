using UnityEngine;
using System.Collections;

public class PrajuritBerbaris : MonoBehaviour {
		
	//private bool hormat = false;
	
	// Use this for initialization
	void Start () {
		animation["sallute_start"].wrapMode = WrapMode.Once;
		animation["sallute_end"].wrapMode = WrapMode.Once;
		animation["sallute_non_stop"].wrapMode = WrapMode.Once;
		
		animation.Play("stand");
		
		//StartCoroutine(hormatGrak());
		
		StartCoroutine(hormatGrak());
		StartCoroutine(tegakGrak());
		StartCoroutine(stand());
		StartCoroutine(SceneSelesai());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	IEnumerator hormatGrak()
	{
		yield return new WaitForSeconds(7);
		animation.Play("sallute_start");
	}
	
	IEnumerator tegakGrak()
	{
		yield return new WaitForSeconds(15f);
		animation.Play("sallute_end");
	}
	
	IEnumerator stand()
	{
		yield return new WaitForSeconds(17f);
		animation.Play("stand");
	}
	
	IEnumerator SceneSelesai()
	{
		yield return new WaitForSeconds(19f);
		// silahkan diarahkan ke scene setelah ini
		Debug.Log("Scene Selesai");
	}
}
