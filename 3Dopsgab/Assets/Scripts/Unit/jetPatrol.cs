using UnityEngine;
using System.Collections;

/// <summary>
/// script jet patrol ini diattach kepada pesawat jet untuk melakukan
/// patroli ke daerah yang ditentukan. tidak ada interaksi dari user
/// untuk menggerakan pesawatnya, user hanya melihat saja
/// </summary>

public class jetPatrol : MonoBehaviour {
	
	// kecepatan pesawat
	public float jetSpeed = 10;
	
	// inisialisasi transform
	Transform myTransform;
	
	// jet target
	public Transform myTarget;
	
	private RaycastHit hit;
	
	private float range = 10;
	
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		myTransform.Translate( Vector3.forward * jetSpeed * Time.deltaTime);
		
		myTransform.LookAt(myTarget);
						
		// jika posisi pesawat sama dengan posisi target
		if( Physics.Raycast(myTransform.position, myTransform.forward, out hit, range))
		{
			if( hit.transform.tag == "target")
			{
				boxMovement.boxDetected = true;
			}
                        
		}
		
		 //patroli telah berakhir
        if( boxMovement.banyakPatroli == true)
        {
            patroliBerakhir();	
        }

        //posisi si pesawat
        //Debug.Log("objek x: " + myTransform.position.x + "objek y: " + myTransform.position.y + "objek z: " + myTransform.position.z);
		
	}
	
	void patroliBerakhir()
	{
		myTarget.position = new Vector3( -19.27001f, 30.05735f, 381.6334f);	
		
		if( Physics.Raycast( myTransform.position, myTransform.forward, out hit, range))
		{
			jetSpeed = 0;
            boxMovement.kecepatanBox = 0;
            boxMovement.rotation = 0;
			myTransform.Rotate(0.0f, 0.0f, 0.0f);	
		}
	}
}