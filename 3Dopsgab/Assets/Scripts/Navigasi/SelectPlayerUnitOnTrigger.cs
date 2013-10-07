using UnityEngine;
using System.Collections;

public class SelectPlayerUnitOnTrigger : MonoBehaviour {
	
	public string[] tags;
	private UnitManager unitManager;
	
	void Start() {
		GameObject unitManagerObject = GameObject.FindGameObjectWithTag("unitmanager");
		unitManager = unitManagerObject.GetComponent<UnitManager>();
	}
	
	void OnTriggerEnter(Collider col) {
		foreach(string tag in tags) {
			if(col.tag == tag) {
				unitManager.SelectAdditionalUnit(gameObject);
				return;
			}
		}
	}
}
