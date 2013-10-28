using UnityEngine;
using System.Collections;
using System;
/*
 * HistoryManager mengatur save-load history game pada editUnitMode, 
 * dan mengatur jalannya playMode.
 */
public class HistoryManager : MonoBehaviour
{

    // the Singleton
    private static HistoryManager instance = null;
    public static HistoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("instantiate");
                GameObject go = new GameObject();
                instance = go.AddComponent<HistoryManager>();
                go.name = "singleton";
            }
            return instance;
        }
    }

    //attributes
    public static bool showHistory = false;
    [DoNotSerialize]
    public static ArrayList historyList = new ArrayList();
    public static int InstanceIdx = 0;
    //operations
    public static string HISTORY_ADD_UNIT = "Tambah unit";
    public static string HISTORY_MOD_UNIT = "Edit unit";
    public static string HISTORY_DEL_UNIT = "Delete unit";
    public static string HISTORY_ADD_WAYPOINT = "Tambah rute";
    public static string HISTORY_MOD_WAYPOINT = "Tambah rute";
    public static string HISTORY_DEL_WAYPOINT = "Tambah rute";
    public static string HISTORY_ADD_TARPOINT = "Tambah target";

    
    //operasi manajemen history
    public static bool addToHistory(string a)
    {
        if (a == null || a == "") return false;
        historyList.Add(a);
        return true;
    }

    public static bool addToHistory(HistoryItem a)
    {
        if (a == null) return false;
        historyList.Add(a);

        //save undo sebagai save game
        LevelSerializer.PlayerName = PlayerPrefs.GetString("satuan");
        LevelSerializer.SaveGame("UnitConfig " + a.ToString());
        if (LevelSerializer.SavedGames.Count >= LevelSerializer.MaxGames)
        {
            LevelSerializer.MaxGames++;
        }
        Debug.Log("saved as: " + a.ToString() + " max saves:" + LevelSerializer.MaxGames + " in: " + Application.persistentDataPath);
        return true;
    }

    public static bool undoHistory(HistoryItem a)
    {
        LevelSerializer.SaveEntry sgret = null;
        //endcobian
        Debug.Log("loading : " + a.ToString());
        foreach (LevelSerializer.SaveEntry sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName])
        {
            //Debug.Log("\t\tsave entries: " + sg.Name);
            //if (sg.Name == a.ToString())
            if (sg.Name == "UnitConfig " + a.ToString())
            {
                sgret = sg;
                //LevelSerializer.LoadNow(sg.Data);
                break;
            }
        }
        if (sgret != null)
        {
            sgret.Load();
            //LevelSerializer.LoadNow(sgret.Data);
            Debug.Log("berhasil Load " + sgret.Name); return true;
        }
        else
        {
            Debug.Log("gagal Load"); return false;
        }
    }


    internal static string getCleanName(Transform myTransform, string which)
    {
        //prepare to add to history
        string name = myTransform.collider.gameObject.name;
        int idxclone = myTransform.collider.gameObject.name.IndexOf("(Clone)");
        string prefabName = (idxclone < 0) ? name : name.Remove(idxclone, "(Clone)".Length);
        int id = myTransform.collider.gameObject.GetInstanceID();
        string newName = (which == "name") ? prefabName+id : (which == "prefab") ? prefabName : "undefined";

        if (which == "name") return newName;
        else if (which == "prefab") return prefabName;
        else return "";
    }
}

public class HistoryItem
{
    public string command;
    public string objectName;
    public string prefabName;
    public Vector3 initialPos;
    public Vector3 finalPos;

    public HistoryItem()
    {
        command = "";
        objectName = "";
        prefabName = "";
        initialPos = Vector3.zero;
        finalPos = Vector3.zero;
    }

    public HistoryItem(string command, string objName, string prefabName, Vector3 initPos)
    {
        this.command = command;
        this.objectName = objName;
        this.prefabName = prefabName;
        this.initialPos = initPos;
        this.finalPos = Vector3.zero;
    }

    public HistoryItem(string command, string objName, string prefabName, Vector3 initPos, Vector3 finalPos)
    {
        this.command = command;
        this.objectName = objName;
        this.prefabName = prefabName;
        this.initialPos = initPos;
        this.finalPos = finalPos;
    }

    public override string ToString()
    {
        return command + " unit \n" + objectName + "\npada " + initialPos.ToString() + (finalPos == Vector3.zero ? "" : " menuju " + finalPos.ToString());
    }

}

