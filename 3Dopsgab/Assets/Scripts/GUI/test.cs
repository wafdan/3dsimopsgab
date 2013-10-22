using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    //public bool doWindow0 = true;
    //void DoWindow0(int windowID) {
        //GUI.Button(new Rect(10, 30, 80, 20), "Click Me!");
		//Application.LoadLevelAdditive("Persiapan Siaga Tempur Udara");
    //}
    //void OnGUI() {
        //doWindow0 = GUI.Toggle(new Rect(10, 10, 100, 20), doWindow0, "Window 0");
        //if (doWindow0)
            //GUI.Window(0, new Rect(50, 10, 640, 480), DoWindow0, "Tampilan");
        
    //}
	void Start() {
		MovieTexture movie = renderer.material.mainTexture as MovieTexture;
		movie.Play();
	}
}