using UnityEngine;
using System.Collections;

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
    public static ArrayList historyList = new ArrayList();
    public static int InstanceIdx = 0;
    //operations
    public static string HISTORY_ADD_UNIT = "Tambah unit";
    public static string HISTORY_MOD_UNIT = "Edit unit";
    public static string HISTORY_DEL_UNIT = "Delete unit";
    public static string HISTORY_ADD_WAYPOINT = "Tambah rute";
    public static string HISTORY_MOD_WAYPOINT = "Tambah rute";
    public static string HISTORY_DEL_WAYPOINT = "Tambah rute";

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
        LevelSerializer.SaveGame(a.ToString());
        LevelSerializer.SerializeLevelToFile("testSaveGame.tfgsg");
        //UnitySerializer.SerializeToFile(LevelSerializer.SavedGames, "somePath");
        Debug.Log("saved as: " + a.ToString() + " max saves:" + LevelSerializer.MaxGames + " in: " + Application.persistentDataPath);
        return true;
    }
    /*
    public static bool undoHistory(){
        if (historyList.Count == 0) return false;
        historyList.RemoveAt(historyList.Count - 1);
        return true;
    }
     */
    public static bool undoHistory(HistoryItem a)
    {
        
        //endcobian
        Debug.Log("loading : " + a.ToString());
        foreach (LevelSerializer.SaveEntry sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName])
        {
            Debug.Log("\t\tsave entries: " + sg.Name);
            if (sg.Name == a.ToString())
            {
                sg.Load();
                //LevelSerializer.LoadNow(sg.Data);
                Debug.Log("berhasil Load");
                return true;
            }
        }
        Debug.Log("gagal Load");
        return false;
    }

    public void OnApplicationQuit()
    {
        //foreach (LevelSerializer.SaveEntry sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName])
        //{
        //    sg.Delete();
        //}
        //Debug.Log("all saved history removed");
    }

    public static HistoryItem[] testOperationArray = {
                                                         new HistoryItem("add","Sukhoi-01","Sukhoi",new Vector3(-114.6f,9f,383.4f)),
                                                         new HistoryItem("add","F16-02","F16",new Vector3(-179f,9f,288f))
                                                     };
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
        return command + " unit " + objectName + "\n pada " + initialPos.ToString() + (finalPos == Vector3.zero ? "" : " menuju " + finalPos.ToString());
    }

}

