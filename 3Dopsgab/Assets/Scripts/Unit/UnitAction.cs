using UnityEngine;
using System.Collections;
using System.Linq;

public class UnitAction : MonoBehaviour {

    public bool UNIT_DEFAULT = false;
    public bool UNIT_AIRFIGHTER = false;
    public bool UNIT_AIRDEFAULT = false;
    public bool UNIT_AIRCARGO = false;
    public bool UNIT_SUBMARINE = false;
    public bool UNIT_BATTLESHIP = false;
    public bool UNIT_TANK = false;

    public static string[] ACTIONS_DEFAULT = new string[] { "Delete" };
    public static string[] ACTIONS_AIRDEFAULT = new string[] { "Mendarat", "Delete" };
    public static string[] ACTIONS_AIRFIGHTER = new string[] { "Set Sasaran Tembak", "Delete" };
    public static string[] ACTIONS_AIRCARGO = new string[] { "Set Titik Terjun", "Delete" };
    public static string[] ACTIONS_SUBMARINE = new string[] { "Set Titik Selam", "Set Titik Apung" };
    public static string[] ACTIONS_BATTLESHIP = new string[] { "Set Sasaran Tembak" };
    public static string[] ACTIONS_TANK = new string[] { "Set Sasaran Tembak" };

    public UnitAction()
    {
        //UNIT_DEFAULT = false;
        //UNIT_AIRFIGHTER = false;
        //UNIT_AIRDEFAULT = false;
        //UNIT_AIRCARGO = false;
        //UNIT_SUBMARINE = false;
    }

    public string[] getActions()
    {
        string[] ret = ACTIONS_DEFAULT;
        if (UNIT_AIRDEFAULT) ret = ret.Union(ACTIONS_AIRDEFAULT).ToArray<string>();
        if (UNIT_AIRFIGHTER) ret = ret.Union(ACTIONS_AIRFIGHTER).ToArray<string>();
        if (UNIT_AIRCARGO) ret = ret.Union(ACTIONS_AIRCARGO).ToArray<string>();
        if (UNIT_SUBMARINE) ret = ret.Union(ACTIONS_SUBMARINE).ToArray<string>();
        if (UNIT_BATTLESHIP) ret = ret.Union(ACTIONS_BATTLESHIP).ToArray<string>();
        if (UNIT_TANK) ret = ret.Union(ACTIONS_TANK).ToArray<string>();
        //else return ACTIONS_DEFAULT;

        return ret;


    }


    void OnDestroy()
    {
        DestroyImmediate(renderer.material);
    }

}
