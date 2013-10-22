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

    void Start()
    {
        StartCoroutine(startTheGameClock());
        StartCoroutine(playTheKegs());
        //if (OperationManager.operationList.Count > 0)
        //{
        //    isPlayTheKegs = true;
        //}
        //if (isPlayTheKegs)
        //{
        //    StartCoroutine(playTheKegs());
        //}
    }

    private IEnumerator playTheKegs()
    {
        Debug.Log("playTheKegs");
        while (true)
        {
            yield return null;
            if (MenuUnit.showPlayMode)
            {
                for (int i = MenuUnit.curOpPlayIdx; i < OperationManager.operationList.Count; i++)//DI SINI MASALAHNYA, SOLUSINYA: SORTING BY DATE
                {
                    yield return null;
                    OperationItem opItIn = (OperationItem)OperationManager.operationList[i];
                    
                    if (opItIn.satuan == PlayerPrefs.GetString("satuan"))
                    {
                        string pos = preProcPosHar(opItIn.posisiHari);
                        
                        DateTime opStartTime = opItIn.startTime;
                        DateTime opDoneTime = opItIn.endTime;

                        if (posHariInGameClock == pos)
                        {
                            
                            if (gameTime.TimeOfDay >= opStartTime.TimeOfDay)// && gameTime < opDoneTime)
                            {
                                
                                if (!nowPlayingList.Contains(opItIn))
                                {
                                    nowPlayingList.Add(opItIn);
                                }
                            }
                        }

                        if (gameTime >= opDoneTime)
                        {
                            if (nowPlayingList.Contains(opItIn))
                            {
                                nowPlayingList.Remove(opItIn);
                                
                            }
                        }
                        
                    }
                    if (!MenuUnit.showPlayMode) { nowPlayingList.Clear(); break; }
                }
            }
            
        }
    }

    OperationItem opFirstPlay;
    OperationItem opItIn;
    DateTime gameTime;
    public static string gameClockValue;
    public static string nowPlaying = "";
    public static ArrayList nowPlayingList = new ArrayList();
    public static float timeSpeed = 0.1f;
    private bool isPlayTheKegs = false;
    private bool isAdded = false;
    private string posHariInGameClock = "";
    private DateTime opFirstPlayStartDate;

    private IEnumerator startTheGameClock()
    {
        while (true)
        {
            yield return null;
            if (MenuUnit.showPlayMode)
            {

                // opIt starting point
                opFirstPlay = (OperationItem)OperationManager.operationList[MenuUnit.curOpPlayIdx];
                // mulai semua kegiatan dari starting point
                gameTime = opFirstPlay.startTime; //timeToIncrement
                //DateTime lastST = DateTime.Parse(opFirstPlay.startTime); //caching hari "kemarin"
                opFirstPlayStartDate = opFirstPlay.startTime;//startTime
                //DateTime eTim = opFirstPlay.startTime.Add(opFirstPlay.duration); //endTime

                string curPosHar = opFirstPlay.posisiHari;

                gameClockValue = opFirstPlay.posisiHari + " " + string.Format("{0}:{1}:{2}", gameTime.Hour, gameTime.Minute, gameTime.Second);
                while (true)
                {
                    yield return null;
                    gameTime = gameTime.AddSeconds(1*timeSpeed);
                    //proses posHar
                    posHariInGameClock = getEndPosHar(curPosHar);

                    gameClockValue = posHariInGameClock + " " + gameTime.ToString("dd/MM/yyyy HH:mm:ss");
                        //+ "\nmulai: " + sTim.ToString("dd/MM/yyyy HH:mm") + "\ntamat: " + eTim.ToString("dd/MM/yyyy HH:mm");

                    if (!MenuUnit.showPlayMode) { break; }

                }//endwhile
            }//endif
        }//endwhile
    }

    private string getEndPosHar(string curPosHar)
    {
        string hPosHar = curPosHar.Substring(2).Replace(" ", ""); //cari string posisi hari bagian angkanya
        //Debug.Log("hposHar: " + hPosHar);
        int posHarInt = 0; //siap2 konversi string jadi int
        Int32.TryParse(hPosHar, out posHarInt); //kalo gagal berarti hari H
        
        int hEnd = posHarInt - (opFirstPlayStartDate.Date - gameTime.Date).Days;
        
        //Debug.Log("selisih hari: "+(lastST - gameTime).Days+" lastST: "+lastST + " hEnd: " + hEnd);
        string hEndStr = "Hari H";
        if (hEnd < 0) { hEndStr = "H" + hEnd; }
        else if (hEnd > 0) { hEndStr = "H+" + hEnd; }

        return hEndStr;
    }

    private string preProcPosHar(string posHar)
    {
        string hPosHar = posHar.Substring(2).Replace(" ", "");
        int posHarInt = 0; 
        Int32.TryParse(hPosHar, out posHarInt);

        string hEndStr = "Hari H";
        if (posHarInt < 0) { hEndStr = "H" + posHarInt; }
        else if (posHarInt > 0) { hEndStr = "H+" + posHarInt; }

        return hEndStr;
    }

    private IEnumerator playIndividualOperation(OperationItem opItIndv)
    {
        DateTime sT = opItIndv.startTime; //timeToIncrement
        DateTime sTim = opItIndv.startTime;//startTime
        DateTime eTim = opItIndv.startTime.Add(opItIndv.duration); //endTime
        string kegClockValue = "";// opIt.posisiHari + " " + string.Format("{0}:{1}:{2}", sT.Hour, sT.Minute, sT.Second);
        Debug.Log("start keg: " + opItIndv.name);
        bool isStart = false;

        while (true)
        {
            if (sT >= gameTime)
            {
                isStart = true;
                eTim = opItIndv.startTime.Add(opItIndv.duration);
            }
            if (isStart)
            {

                sT = sT.AddSeconds(1 * timeSpeed);
                kegClockValue = opItIndv.posisiHari + " " + sT.ToString("dd/MM/yyyy HH:mm")
                    + "\nmulai: " + sTim.ToString("dd/MM/yyyy HH:mm") + "\ntamat: " + eTim.ToString("dd/MM/yyyy HH:mm");

                //Debug.Log(kegClockValue);
                if (sT >= eTim)
                {
                    Debug.Log("keg: " + opItIndv.name + " finished at "+sT.ToString());
                    break;
                }
            }
            yield return null;//new WaitForSeconds(0);
        }
        //Debug.Log("keg: " + opIt.name+" finished.");
    }

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

