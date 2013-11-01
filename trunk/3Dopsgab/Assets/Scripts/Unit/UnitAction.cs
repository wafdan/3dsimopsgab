using UnityEngine;
using System.Collections;
using System.Linq;

public class UnitAction : MonoBehaviour {

    public bool UNIT_DEFAULT;
    public bool UNIT_AIRFIGHTER;
    public bool UNIT_AIRDEFAULT;
    public bool UNIT_AIRCARGO;
    
    public static string[] ACTIONS_DEFAULT = new string[] { "Delete" };
    public static string[] ACTIONS_AIRDEFAULT = new string[] { "Mendarat", "Delete" };
    public static string[] ACTIONS_AIRFIGHTER = new string[] { "Set Sasaran Tembak", "Delete" };
    public static string[] ACTIONS_AIRCARGO = new string[] { "Set Titik Terjun", "Delete" };

    public UnitAction()
    {
        UNIT_DEFAULT = false;
        UNIT_AIRFIGHTER = false;
        UNIT_AIRDEFAULT = false;
        UNIT_AIRCARGO = false;
    }

    public string[] getActions()
    {
        string[] ret = ACTIONS_DEFAULT;
        if (UNIT_AIRDEFAULT) ret = ret.Union(ACTIONS_AIRDEFAULT).ToArray<string>();
        if (UNIT_AIRFIGHTER) ret = ret.Union(ACTIONS_AIRFIGHTER).ToArray<string>();
        if (UNIT_AIRCARGO) ret = ret.Union(ACTIONS_AIRCARGO).ToArray<string>();
        //else return ACTIONS_DEFAULT;

        return ret;


    }

}
