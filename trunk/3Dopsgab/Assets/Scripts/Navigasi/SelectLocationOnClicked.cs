using UnityEngine;
using System.Collections;

public class SelectLocationOnClicked : MonoBehaviour {
	
    //private UnitManager unitManager;
   //private CameraMovement cam;
	
   //void Start() {
   //    GameObject unitManagerObject = GameObject.FindGameObjectWithTag("unitmanager");
   //    unitManager = unitManagerObject.GetComponent<UnitManager>();
   //}
    void Clicked()
    {
        //Application.LoadLevel(1);
        Application.LoadLevelAdditive("Sangatta Serang");

        //camera.cullingMask = ~(1 << LayerMask.NameToLayer("Water"));

        //BasicUnitMovement.isSelected = true;
        //if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        //{
        //    unitManager.SelectAdditionalUnit(gameObject);

        //}
        //else
        //{
        //    unitManager.SelectSingleUnit(gameObject);
        //}
    }
}
