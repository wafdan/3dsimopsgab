using UnityEngine;
using System.Collections;

public class EmbarkasiTank : MonoBehaviour {
	
	public Transform[] wayPoint;
	private Transform myTransform;
	public Transform[] myTarget;
	
	public float tankSpeed = 20;
	private bool loop = false;	
	
	private int currentWaypoint;
		
	void Awake()
	{
		wayPoint[0] = transform;	
	}
	
	// Use this for initialization
	void Start () {
		myTransform = transform;		
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 velocity = rigidbody.velocity;
		
		if( currentWaypoint < wayPoint.Length)
		{
			Vector3 target = wayPoint[currentWaypoint].position;
			Vector3 moveDirection = target - myTransform.position;
			
			
			if( moveDirection.magnitude < 1)
			{
				//myTransform.Rotate(0,-210,0);
				currentWaypoint++;
			}
			else
			{
				velocity = moveDirection.normalized * tankSpeed;	
				myTransform.LookAt(myTarget[currentWaypoint]);
			}
		}
		
		else
		{
			if( loop)
			{
				currentWaypoint = 0;	
			}
			else
				velocity = Vector3.zero;
		}
		
		rigidbody.velocity = velocity;		
	}
	
	void OnTriggerEnter( Collider otherObject)
	{
		if( otherObject.transform.tag == "pintu")
		{
			Debug.Log("im in");	
			StartCoroutine(DestroyTank());
		}
	}
	
	IEnumerator DestroyTank()
	{
		yield return new WaitForSeconds(1);
		Destroy(myTransform.gameObject);
	}
}
