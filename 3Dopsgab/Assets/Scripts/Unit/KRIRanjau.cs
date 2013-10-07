using UnityEngine;
using System.Collections;

/// <summary>
/// KRI ranjau, di attach ke KRI ranjau.
/// </summary>

public class KRIRanjau : MonoBehaviour {
	
	private Transform cameraHeadTransform;
	private Transform myTransform;
	
	public Transform ranjau;
	
	private RaycastHit hit;
	private float range = 1.5f;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		cameraHeadTransform = myTransform.FindChild("CameraHead");
	}
	
	// Update is called once per frame
	void Update () {
		
		if( RanjauHancur.readyForExploded == false)
		{
			pergerakan();	
		}
		
		if( Physics.Raycast(myTransform.position, myTransform.forward, out hit, range))
		{
			if( hit.transform.tag == "ranjau")
			{
				RanjauHancur.readyForExploded = true;
			}
		}
	}
	
	void pergerakan()
	{
		myTransform.LookAt(ranjau);
		
		myTransform.Translate(Vector3.forward * 1 * Time.deltaTime);	
	}
}
