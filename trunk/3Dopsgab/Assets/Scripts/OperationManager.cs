using UnityEngine;
using System.Collections;
using System;
using System.IO;
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
    public static DateTime hariH = DateTime.Now;
    public static int InstanceIdx = 0;
    public static string FILE_EXT=".tfgsg";

    public void Start()
    {
        StartCoroutine(playOperation());
    }

    public static void setHariH(DateTime newHariH)
    {
        hariH = newHariH;
    }

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

    public static void playOperations()
    {

    }

    public void OnApplicationQuit()
    {
        //foreach (LevelSerializer.SaveEntry sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName])
        //{
        //    sg.Delete();
        //}
        //Debug.Log("all saved operation removed");
    }


    public static IEnumerator playOperation()
    {
        while (true)
        {
            //cek if playmode, do this..
            //cek start mulai playnya dari mana
            //mulai dari posisi itu
            //IF otomatis, looping for posisi itu hingga akhir di operationList 
            //  IF kondisi satuan, hari, sesuai,
            //  play dengan waktu tertentu, misal fix 10 detik per slide.
            //  oh ya ada tombol "next" dan "prev" untuk play mode.
            //  IF hasUnit
            //    posisikan GUI di pojok kanan atas, kecil.
            //    testEksekusi = true (Coroutine?)
            //  IF hasVids
            //    testEksekusi = false;
            //    playVids... (Coroutine?)
            //  getNextOperation..
            //  endloop

            // pada akhirnya, playmode = false
            yield return null;
        }
    }

    public static void saveGameToFile(string filename)
    {
        LevelSerializer.SerializeLevelToFile(filename);
        LevelSerializer.SavedGames.Clear();
        HistoryManager.historyList.Clear();
        Debug.Log("Saved as " + filename);
    }

    public static bool deleteSavedGame(string filename)
    {
        try
        {
            // A.
            // Try to delete the file.
            File.Delete(filename);
            Debug.Log("berhasil delete");
            return true;
        }
        catch (IOException)
        {
            // B.
            // We could not delete the file.
            Debug.Log("gagal delete");
            return false;
        }
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
    public UnityEngine.Object unitConfig;
    public string startTime;
    public TimeSpan duration;
    public string endTime;
    public bool hasUnitMovement;
    public bool hasVideo;

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

    public OperationItem(string satuan, string posHar, string nama, string lokasi, string deskripsi, UnityEngine.Object newUnitConfig)
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

    public OperationItem(string satuan, string posHar, string nama, string lokasi, string deskripsi, string file, UnityEngine.Object newUnitConfig)
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

    //yg dipake
    public OperationItem(string satuan, string posHar, string nama, string lokasi, string deskripsi, string file, UnityEngine.Object newUnitConfig,string startTime, TimeSpan duration, bool hasFile, bool hasUnit)
    {
        this.satuan = satuan;
        this.posisiHari = posHar;
        this.name = nama;
        this.location = lokasi;
        this.description = deskripsi;
        this.locationPoint = Vector3.zero;
        this.files = file;
        this.unitConfig = newUnitConfig;
        this.startTime = startTime;
        this.duration = duration;
        string format = "dd/MM/yyyy HH:mm";
        endTime = DateTime.Parse(startTime).Add(duration).ToString(format);
        this.hasVideo = hasFile;
        this.hasUnitMovement = hasUnit;
    }

    public override string ToString()
    {
        return name + "\nLokasi: " + location+"\nSatuan: "+satuan;
    }

}

