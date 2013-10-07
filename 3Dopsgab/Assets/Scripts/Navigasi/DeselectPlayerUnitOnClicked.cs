using UnityEngine;
using System.Collections;

public class DeselectPlayerUnitOnClicked : MonoBehaviour {
	
	private UnitManager unitManager;
	
	void Start() {
		GameObject unitManagerObject = GameObject.FindGameObjectWithTag("unitmanager");
		unitManager = unitManagerObject.GetComponent<UnitManager>();
	}
	
//	void Clicked() {
//		unitManager.DeselectAllUnits();
//	}
}
