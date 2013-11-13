using UnityEngine;
using System.Collections;

public class SelectLocationOnClicked : MonoBehaviour
{

    Transform myTransform;

    void Awake()
    {
        myTransform = transform;
    }
    //private UnitManager unitManager;
    //private CameraMovement cam;

    //void Start() {
    //    GameObject unitManagerObject = GameObject.FindGameObjectWithTag("unitmanager");
    //    unitManager = unitManagerObject.GetComponent<UnitManager>();
    //}
    void Clicked()
    {
        //Application.LoadLevelAdditive("Sangatta1000");
        if (myTransform.childCount > 0)
        {
            for (int i = 0; i < myTransform.childCount; i++)
            {
                switch (myTransform.GetChild(i).name)
                {
                    case "Tarakan":
                        Application.LoadLevel("DaratTest");
                        break;
                    case "Sangatta":
                        Application.LoadLevel("Sangatta1000");
                        break;
                    default:
                        break;
                }

            }
        }
        //Application.LoadLevelAdditive("Sangatta Serang");

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
