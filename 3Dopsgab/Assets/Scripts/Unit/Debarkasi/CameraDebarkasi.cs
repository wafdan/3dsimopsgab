using UnityEngine;
using System.Collections;

public class CameraDebarkasi : MonoBehaviour {
	
	private Transform myTransform;
	public Transform myTarget;
	public float cameraSpeed = 5;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		myTransform.LookAt(myTarget);
		
		StartCoroutine(GantiScene());
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
		
	}
	
	IEnumerator GantiScene()
	{
		yield return new WaitForSeconds(28);
		Debug.Log("ganti scene");
	}
}
