using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviourForward : BehaviourBase {

	private Vector3 direction;
	public BehaviourForward(float weight, Actor caller, Vector3 forwardDirection) : base(weight, caller) {
		this.direction = forwardDirection;
	}
	
	public override void Update() {
		this.caller.transform.LookAt(direction);
		this.caller.transform.Translate(direction * this.caller.speed * this.weight * Time.deltaTime);
	}
}
