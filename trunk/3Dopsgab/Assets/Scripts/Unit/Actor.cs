using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {

	public static List<Actor> Actors = new List<Actor>();
	public float speed = 2;
	public List<BehaviourBase> behaviours = new List<BehaviourBase>();
	
	void Start() {		
		behaviours.Add(new SingleWayPoint(1, this, GameObject.Find("bmp3fa")));
	}
	
	void Update() {
		foreach(BehaviourBase b in behaviours){
			b.Update();
		}
	}
}
