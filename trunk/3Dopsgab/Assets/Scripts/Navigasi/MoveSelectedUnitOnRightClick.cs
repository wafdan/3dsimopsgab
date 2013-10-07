using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveSelectedUnitOnRightClick : MonoBehaviour
{

    //public GameObject moveEffectObject;
    //private UnitManager unitManager;
    //Vector3 posisi;
    //private bool debug = true;
    //string seluns = "";

    //void Start()
    //{
    //    GameObject unitManagerObject = GameObject.FindGameObjectWithTag("unitmanager");
    //    unitManager = unitManagerObject.GetComponent<UnitManager>();
    //}

    //void RightClicked(Vector3 clickPosition)
    //{
    //    foreach (GameObject unit in unitManager.GetSelectedUnits())
    //    {
    //        if (BasicUnitMovement.checkIfGoalSame(transform.position, clickPosition))
    //        {
    //            unit.SendMessage("MoveOrder", clickPosition);
    //            Instantiate(moveEffectObject, clickPosition, moveEffectObject.transform.rotation);
    //            BasicUnitMovement.action = true;
    //        }
    //    }
    //}

    //void OnGUI()
    //{
    //    if (debug)
    //    {
    //        List<GameObject> units = unitManager.GetSelectedUnits();
    //        seluns = "";
    //        foreach (GameObject unit in units)
    //        {
    //            seluns += unit.name+"\n";
    //        }
    //        GUI.Box(new Rect(100, 100, 300, 400), "Selected Unit ("+units.Count+"):\n" + seluns);
    //    }
    //}

}
