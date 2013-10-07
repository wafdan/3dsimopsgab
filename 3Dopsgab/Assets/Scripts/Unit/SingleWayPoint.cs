using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleWayPoint : BehaviourBase {
	
	private GameObject Waypoint;
	public SingleWayPoint(float weight, Actor caller, GameObject wp) : base(weight, caller) {
		this.Waypoint = wp;
	}

	public override void Update() {
		Quaternion rot = Quaternion.LookRotation((this.Waypoint.transform.position - this.caller.transform.position).normalized);
		this.caller.transform.rotation = Quaternion.Slerp(this.caller.transform.rotation, rot, Time.deltaTime * 4.5f);
		
		if(Vector3.Distance(this.caller.transform.position, this.Waypoint.transform.position) >= 1) {
			this.caller.rigidbody.velocity = this.caller.transform.forward * weight * this.caller.speed;
		}
	}
}
