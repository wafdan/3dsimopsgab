using UnityEngine;
using System.Collections;

/// <summary>
/// Ranjau hancur, script ini diattach pada ranjau.
/// jadi ketika kapal sudah mendeteksi keberadaan ranjau ini, 
/// ranjau meledak (instansiate partikel). setelah itu, kapal
/// melaju ke ranjau yang lain.
/// </summary>

public class RanjauHancur : MonoBehaviour {
	
	private Transform myTransform;
	
	public Transform explosion;
	
	public static bool readyForExploded;
	
	private RaycastHit hit;
	private float range = 3.0f;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		readyForExploded = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if( readyForExploded == true)
		{
			ledakan();
		}
	}
	
	void ledakan()
	{
		myTransform.Translate(Vector3.down * 0.5f * Time.deltaTime);
		
		readyForExploded = false;
		
		if( myTransform.position.y < 1.32f)
		{
			Instantiate(explosion, myTransform.position, Quaternion.identity);
			Destroy(myTransform.gameObject);
		}
		
	}
	
}
