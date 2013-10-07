using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviourBase : ScriptableObject {

	protected float weight;
	protected Actor caller;
	
	public BehaviourBase(float weight, Actor caller) {
		this.weight = weight;
		this.caller = caller;
	}
	
	public virtual void Update() {}
}
