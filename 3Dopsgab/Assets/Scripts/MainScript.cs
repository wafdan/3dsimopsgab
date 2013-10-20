using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {
	// the Singleton
	private static MainScript instance = null;
	public static MainScript Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("instantiate");
                GameObject go = new GameObject();
                instance = go.AddComponent<MainScript>();
                go.name = "singleton";
            }

            return instance;
        }
    }
	
	public static string SAT1 = "KOGAB";
    public static string SAT2 = "KOHANUDNAS";
    public static string SAT3 = "KOGASGABUD";
    public static string SAT4 = "KOGASGABLA";
    public static string SAT5 = "KOGASGABFIB";
    public static string SAT6 = "KOGASGAB LINUD";
    public static string SAT7 = "KOGASGABRAT";
    public static string SAT8 = "KOGASGAB RATMIN";
    public static string SAT9 = "KOSATGAS TER";
    public static string SAT10 = "KOSATGAB INTEL TIS";
    public static string SAT11 = "KOSATGAS PASSUS";
	
	public static string activeClient = SAT1;
	
	public static Hashtable unitOrders = new Hashtable(){
		{"Sukhoi", new UnitOrder("Sukhoi", SAT2, new string[]{"Combat Air Patrol","Ferry","Red Plan","Landing"})},
		{"NAS-332", new UnitOrder("NAS-332", SAT3, new string[]{"Combat Air Patrol","Ferry","Red Plan","Landing"})}
	};
	
}

public class UnitOrder {
	public string name;
	public string kesatuan;
	public string[] orderList;

    public UnitOrder()
    {
        this.name = "";
        this.kesatuan = "";
        this.orderList = new string[]{};
    }

	public UnitOrder(string name, string kesatuan, string[] orderList){
		this.name = name;
		this.kesatuan = kesatuan;
		this.orderList = orderList;
	}
	
}

public class UnitInfo
{
    public string name;
    public Texture2D texture;
    public GameObject building;

    public UnitInfo()
    {
        name = "";
        texture = null;
        building = null;
    }
    public UnitInfo(string n, Texture2D t, GameObject g)
    {
        name = n;
        texture = t;
        building = g;
    }
}
