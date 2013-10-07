using UnityEngine;
using System.Collections;
[SerializeAll]
public class OperationManager : MonoBehaviour
{

    // the Singleton
    private static OperationManager instance = null;
    public static OperationManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("instantiate");
                GameObject go = new GameObject();
                instance = go.AddComponent<OperationManager>();
                go.name = "singleton";
            }
            return instance;
        }
    }

    //attributes

    public static ArrayList operationList = new ArrayList();
    public static int InstanceIdx = 0;

    public static bool addToOperationList(OperationItem a)
    {
        if (a == null) return false;
        operationList.Add(a);
        //LevelSerializer.SaveGame(a.ToString());
        Debug.Log("tambah kegiatan: " + a.ToString()+" max saves:"+LevelSerializer.MaxGames);
        return true;
    }
    /*
	public static bool undoHistory(){
        if (historyList.Count == 0) return false;
        historyList.RemoveAt(historyList.Count - 1);
        return true;
	}
     */
    public static bool undoOperation(OperationItem a)
    {
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
        //Debug.Log("all saved operation removed");
    }


    public static void playOperation(OperationItem p)
    {

    }
}

public class OperationItem
{
    public string satuan;
    public string posisiHari;
    public string name;
    public string location;
    public Vector3 locationPoint;
    public string description;
    public string files;
    public Object unitConfig;
    public string startTime;
    public string endTime;

    public OperationItem()
    {
        satuan = "";
        posisiHari = "";
        name = "";
        location = "";
        locationPoint = Vector3.zero;
        description = "";
        files = "";
        unitConfig = null;
        startTime = "";
        endTime = "";
    }

    public OperationItem(string satuan, string posHar, string nama, string lokasi, string deskripsi)
    {
        this.satuan = satuan;
        this.posisiHari = posHar;
        this.name = nama;
        this.location = lokasi;
        this.description = deskripsi;
        this.locationPoint = Vector3.zero;
        this.files = "";
        this.unitConfig = null;
        startTime = "00:00";
        endTime = "00:00";
    }

    public OperationItem(string satuan, string posHar, string nama, string lokasi, string deskripsi, string file, Object newUnitConfig)
    {
        this.satuan = satuan;
        this.posisiHari = posHar;
        this.name = nama;
        this.location = lokasi;
        this.description = deskripsi;
        this.locationPoint = Vector3.zero;
        this.files = file;
        this.unitConfig = newUnitConfig;
        startTime = "00:00";
        endTime = "00:00";
    }

    public OperationItem(string satuan, string posHar, string nama, string lokasi, string deskripsi, Object newUnitConfig)
    {
        this.satuan = satuan;
        this.posisiHari = posHar;
        this.name = nama;
        this.location = lokasi;
        this.description = deskripsi;
        this.locationPoint = Vector3.zero;
        this.files = "";
        this.unitConfig = newUnitConfig;
        startTime = "00:00";
        endTime = "00:00";
    }

    public override string ToString()
    {
        return name + "\nlokasi: " + location;
    }

}

