using UnityEngine;
using System.Collections;

public class SelectPlayerUnitOnClicked : MonoBehaviour {
	
   private UnitManager unitManager;
	
   void Start() {
       GameObject unitManagerObject = GameObject.FindGameObjectWithTag("unitmanager");
       unitManager = unitManagerObject.GetComponent<UnitManager>();
   }
   void Clicked() {
	Application.LoadLevel(1);	
    Application.LoadLevelAdditive("Sangatta Serang");
	
	//    BasicUnitMovement.isSelected = true;
    //    if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
    //        unitManager.SelectAdditionalUnit(gameObject);
			
    //    }
    //    else {
    //        unitManager.SelectSingleUnit(gameObject);
    //    }
    }
}
