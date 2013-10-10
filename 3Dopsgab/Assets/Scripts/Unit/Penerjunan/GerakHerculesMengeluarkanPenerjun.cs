using UnityEngine;
using System.Collections;

/// <summary>
/// Hercules movement, script ini ntar di attach ke fbx pesawat hercules
/// yang akan menerjunkan beberapa orang di lokasi yang telah ditentukan.
/// skenarionya adalah, gerak hercules adalah lurus, ketika akan mencapai
/// lokasi penerjun, kecepatannya akan berkurang sedikit, baru menerjunkan
/// sekitar 10 orang. hercules tersebut langsung menuju keluar map dan tidak
/// balik lagi.
/// </summary>

public class GerakHerculesMengeluarkanPenerjun : MonoBehaviour {
	
	public float kecepatanHercules = 100;
	private bool terjunkan = false;
	public int banyakPenerjun = 1;
	
	private Transform myTransform;
	public Transform penerjun;
	
	private float ratePenerjun = 0.4f;
	private float nextPenerjun = 0;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if( Time.time > 0.0 && Time.time < 3)
		{
			//terjunkan = false;
			kecepatanHercules = 100;
		}
		
		if( Time.time >= 5.0f && Time.time < 9.0f)
		{
			//terjunkan = true;
			//kecepatanHercules = 70;
			
			if( Time.time > nextPenerjun)
			{
				nextPenerjun = Time.time + ratePenerjun;
			
				Instantiate( penerjun, myTransform.position, myTransform.rotation);
			}
					
		}
		
				 	
		if( myTransform.position.x < -500)
		{
			Destroy(myTransform.gameObject);
			
		}
		
		myTransform.Translate(Vector3.forward * kecepatanHercules * Time.deltaTime);
		Debug.Log(kecepatanHercules);
	}
	
	void TurunkanPenerjun() 
	{
		
	}
	
}
