using UnityEngine;
using System.Collections;

/// <summary>
/// Gerak kamera hercules, script ini di attach di kamera untuk scene penerjunan dari 
/// pesawat hercules. tampilan diperlukan untuk memberikan user experience yang berbeda
/// </summary>
/// 
public class gerakKameraHercules : MonoBehaviour {
	
	public float kecepatanKamera = 10;
	
	public Transform myTarget;
	
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		myTransform.LookAt(myTarget);
		
		myTransform.Translate(Vector3.up * kecepatanKamera * Time.deltaTime);
	}
}
