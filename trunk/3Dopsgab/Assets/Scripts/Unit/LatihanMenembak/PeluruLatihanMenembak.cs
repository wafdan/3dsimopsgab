using UnityEngine;
using System.Collections;

public class PeluruLatihanMenembak : MonoBehaviour {
	
	private Transform myTransform;
	public Transform myBulletExplosion;
	
	private RaycastHit hit;
	private float range	= 1f;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
		StartCoroutine(DestroyBullet());
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Translate(Vector3.forward * 30 * Time.deltaTime);
		
		if( Physics.Raycast(myTransform.position, myTransform.forward, out hit, range))
		{
			if( hit.transform.tag == "tembok")
			{
				Debug.Log("kena tembok");	
				myTransform.renderer.enabled = false;
				Instantiate(myBulletExplosion, myTransform.position, Quaternion.identity);
			}
		}
	}
	
	IEnumerator DestroyBullet()
	{
		yield return new WaitForSeconds(1);
		Destroy(myTransform.gameObject);
	}
	
	
}
