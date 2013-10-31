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
	
    //Misc Functions
    /* katanya sih lebih cepet dari implementasi aslinya Unity. copyright http://pastebin.com/7wnvR4se */
    public static float myInverseLerp(float from, float to, float value)
    {
        if (from < to)
        {
            if (value < from)
            {
                return 0f;
            }
            else if (value > to)
            {
                return 1f;
            }
            else
            {
                return (value - from) / (to - from);
            }
        }
        else
        {
            if (from <= to)
            {
                return 0f;
            }
            else if (value < to)
            {
                return 1f;
            }
            else if (value > from)
            {
                return 0f;
            }
            else
            {
                return 1f - (value - to) / (from - to);
            }
        }
    }

    public static string[] Find_Common_String(string[] p1, string[] p2)
    {
        int count = 0;
        for (int i = 0; i < p1.Length; i++)
        {
            for (int j = 0; j < p2.Length; j++)
            {
                if (p1[i] == p2[j])
                {
                    count++;
                    break;
                }
            }
        }

        string[] result = new string[count];
        count = 0;
        for (int i = 0; i < p1.Length; i++)
        {
            for (int j = 0; j < p2.Length; j++)
            {
                if (p1[i] == p2[j])
                {
                    result[count++] = p1[i];
                    break;
                }
            }
        }

        return result;
    }
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
    //public string[] actions;

    public UnitInfo()
    {
        name = "";
        texture = null;
        building = null;
        //actions = new string[]{};
    }
    public UnitInfo(string n, Texture2D t, GameObject g)
    {
        name = n;
        texture = t;
        building = g;
    }
    //public UnitInfo(string n, Texture2D t, GameObject g,string[] acts)
    //{
    //    name = n;
    //    texture = t;
    //    building = g;
    //    actions = acts;
    //}
}
