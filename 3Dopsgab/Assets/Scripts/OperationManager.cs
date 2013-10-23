using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text.RegularExpressions;
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
    public static string FILE_EXT=".tfgsg";

    public static DateTime HARI_HA = new DateTime(2007, 10, 1, 00, 00, 00);

    //PLAY MODE RELATED
    OperationItem opFirstPlay; //kegiatan yg diklik-play oleh user
    private DateTime opFirstPlayStartDate; //startTime opFirstPlay
    
    DateTime gameClockTime; // clock virtual
    public static string gameClockValue; // nilai clock, dipake di GUI
    public static float gameClockSpeed = 0.1f; // kecepatan default clock
    private string posHariInGameClock = ""; //posisi hari berdasarkan clock

    public static ArrayList nowPlayingList = new ArrayList(); // list kegiatan yg sedang aktif saat clock berjalan

    void Start()
    {
        StartCoroutine(startTheGameClock());
        StartCoroutine(playTheKegs());
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

                            if (gameClockTime.TimeOfDay >= opStartTime.TimeOfDay)// && gameTime < opDoneTime)
                            {

                                if (!nowPlayingList.Contains(opItIn))
                                {
                                    nowPlayingList.Add(opItIn);
                                }
                            }
                        }

                        if (gameClockTime >= opDoneTime)
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
                gameClockTime = opFirstPlay.startTime; //timeToIncrement
                //DateTime lastST = DateTime.Parse(opFirstPlay.startTime); //caching hari "kemarin"
                opFirstPlayStartDate = opFirstPlay.startTime;//startTime
                //DateTime eTim = opFirstPlay.startTime.Add(opFirstPlay.duration); //endTime

                string curPosHar = opFirstPlay.posisiHari;

                gameClockValue = opFirstPlay.posisiHari + " " + string.Format("{0}:{1}:{2}", gameClockTime.Hour, gameClockTime.Minute, gameClockTime.Second);
                while (true)
                {
                    yield return null;
                    gameClockTime = gameClockTime.AddSeconds(1 * gameClockSpeed);
                    //proses posHar
                    posHariInGameClock = getEndPosHar(curPosHar);

                    gameClockValue = posHariInGameClock + " " + gameClockTime.ToString(" HH:mm:ss");//("dd/MM/yyyy HH:mm:ss");
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

        int hEnd = posHarInt - (opFirstPlayStartDate.Date - gameClockTime.Date).Days;

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
            if (sT >= gameClockTime)
            {
                isStart = true;
                eTim = opItIndv.startTime.Add(opItIndv.duration);
            }
            if (isStart)
            {

                sT = sT.AddSeconds(1 * gameClockSpeed);
                kegClockValue = opItIndv.posisiHari + " " + sT.ToString("dd/MM/yyyy HH:mm")
                    + "\nmulai: " + sTim.ToString("dd/MM/yyyy HH:mm") + "\ntamat: " + eTim.ToString("dd/MM/yyyy HH:mm");

                //Debug.Log(kegClockValue);
                if (sT >= eTim)
                {
                    Debug.Log("keg: " + opItIndv.name + " finished at " + sT.ToString());
                    break;
                }
            }
            yield return null;//new WaitForSeconds(0);
        }
        //Debug.Log("keg: " + opIt.name+" finished.");
    }


    //END PLAY MODE RELATED

    public static void setHariH(DateTime newHariH)
    {
        HARI_HA = newHariH;
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

public class OperationItem: IComparable
{
    public string satuan;
    public string posisiHari;
    public string name;
    public string location;
    public Vector3 locationPoint;
    public string description;
    public string files;
    public UnityEngine.Object unitConfig;
    //public string startTime;
    public DateTime startTime;
    public TimeSpan duration;
    //public string endTime;
    public DateTime endTime;
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
        startTime = OperationManager.HARI_HA;
        endTime = OperationManager.HARI_HA;
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
        startTime = OperationManager.HARI_HA;
        endTime = OperationManager.HARI_HA;
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
        startTime = OperationManager.HARI_HA;
        endTime = OperationManager.HARI_HA;
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
        startTime = OperationManager.HARI_HA;
        endTime = OperationManager.HARI_HA;
    }

    //yg dipake
    public OperationItem(string satuan, string posHar, string nama, string lokasi, string deskripsi, string file, UnityEngine.Object newUnitConfig, string startTime, TimeSpan duration, bool hasFile, bool hasUnit)
    {
        this.satuan = satuan;
        this.posisiHari = posHar;
        this.name = nama;
        this.location = lokasi;
        this.description = deskripsi;
        this.locationPoint = Vector3.zero;
        this.files = file;
        this.unitConfig = newUnitConfig;
        this.duration = duration;
        //proses starttime dan endtime
        prosesStartAndEndTime(startTime, posHar, duration);

        this.hasVideo = hasFile;
        this.hasUnitMovement = hasUnit;
    }

    public void prosesStartAndEndTime(string startTime, string posisiHari,TimeSpan durasi)
    {
        int jamStart = 0;
        Int32.TryParse(startTime.Split(':')[0],out jamStart);

        int menitStart = 0;
        Int32.TryParse(startTime.Split(':')[1], out menitStart);

        this.startTime = OperationManager.HARI_HA.AddDays(getPosHarInt(posisiHari)).AddHours(jamStart).AddMinutes(menitStart);
        this.endTime = this.startTime.Add(durasi);

    }

    public int getPosHarInt()
    {
        string hPosHar = this.posisiHari.Substring(2).Replace(" ", "");
        int posHarInt = 0;
        Int32.TryParse(hPosHar, out posHarInt);

        return posHarInt;
    }

    public int getPosHarInt(string posisiHari)
    {
        string hPosHar = posisiHari.Substring(2).Replace(" ", "");
        int posHarInt = 0;
        Int32.TryParse(hPosHar, out posHarInt);

        return posHarInt;
    }

    public string getStartTimeString()
    {
        string r = "";
        int posHarInt = this.getPosHarInt();
        r += ((posHarInt<0)?("H"+posHarInt):(posHarInt>0)?("H+"+posHarInt):"hari H");
        r += " pukul ";
        r += this.startTime.ToString("HH:mm");
        return r;
    }

    public string getEndTimeString()
    {
        string r = "";
        int posHarInt = this.getPosHarEndInt();
        r += ((posHarInt < 0) ? ("H" + posHarInt) : (posHarInt > 0) ? ("H+" + posHarInt) : "hari H");
        r += " pukul ";
        r += this.endTime.ToString("HH:mm");
        return r;
    }

    private int getPosHarEndInt()
    {
        return (int)(this.endTime.Date - OperationManager.HARI_HA.Date).TotalDays;// -1;
    }

    public override string ToString()
    {
        return name + "\nLokasi: " + location+"\nSatuan: "+satuan;
    }

    int IComparable.CompareTo(object obj) //ascending
    {
        OperationItem oi = (OperationItem)obj;
        if (this.getPosHarInt() < oi.getPosHarInt())
        {
            return -1;
        }
        else if (this.getPosHarInt() > oi.getPosHarInt())
        {
            return 1;
        }
        else
        {// harinya sama
            DateTime thisStartTime = this.startTime;
            DateTime oiStartTime = oi.startTime;
            if (thisStartTime.CompareTo(oiStartTime) <0)
            {
                return -1;
            }
            else if (thisStartTime.CompareTo(oiStartTime) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}

class OperationItem_SortByHariStartTimeAscending : IComparer
{
    int System.Collections.IComparer.Compare(object xx, object yy)
    {
        OperationItem x = (OperationItem)xx;
        OperationItem y = (OperationItem)yy;
        if (x.getPosHarInt() > y.getPosHarInt()) return 1;
        else if (x.getPosHarInt() < y.getPosHarInt()) return -1;
        else
        {
            DateTime xStartTime = x.startTime;
            DateTime yStartTime = y.startTime;
            if (xStartTime.CompareTo(yStartTime) < 0)
                return -1;
            else if (xStartTime.CompareTo(yStartTime) > 0)
                return 1;
            else
                return 0;
        }
    }
}
