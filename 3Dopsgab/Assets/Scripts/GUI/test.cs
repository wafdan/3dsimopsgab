using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    public bool notif = true;
    void DoWindow0(int windowID) {
        //GUI.Button(new Rect(10, 30, 80, 20), "Click Me!");
		GUI.Label(new Rect(10,20,310,20),"Periksa kembali nama satuan dan password anda!");
    }
    void OnGUI() {
        //notif = GUI.Button(new Rect(10, 10, 100, 20), "OK");
		//doWindow0 = !doWindow0;
        if (GUI.Button(new Rect(10, 10, 100, 20), "OK")){
			notif =!notif;
		}
		if (!notif){
            GUI.Label(new Rect(100, 50, 310, 50), "Peringatan : ");
		}
    }
}