using UnityEngine;
using System.Collections;

public class KRIAngkutDebarkasi : MonoBehaviour {
	
	private Transform myTransform;
	public Transform theAmphibi;
	public float hold;
	
	private Transform ampibiSource;
	private Vector3 posisiKeluarAmphibi = new Vector3();
		
	private float rateAmpibi = 8f;
	private float nextAmpibi = 0;
	
	private float random;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		ampibiSource = myTransform.FindChild("KeluarAmpibi");
		
		random = Random.Range(5,9);
		
		StartCoroutine(BukaGerbang());
	}
	
	// Update is called once per frame
	void Update () {
		if( Time.time > nextAmpibi && Time.time > random && Time.time < 30)
		{
			nextAmpibi = rateAmpibi + Time.time;
			
			//posisiKeluarAmphibi = myTransform.TransformPoint(0.0003590223f, 0.0006117127f, -0.07635416f);
			
			posisiKeluarAmphibi = ampibiSource.position;
			
			Instantiate(theAmphibi, posisiKeluarAmphibi, Quaternion.identity);
		}
	}
	
	IEnumerator BukaGerbang()
	{
		yield return new WaitForSeconds(hold);
		animation.Play("door_opening");
	}
	
	IEnumerator AmpibiKeluar()
	{
		yield return new WaitForSeconds(hold + 5);
		// ampibi keluar
	}
}
