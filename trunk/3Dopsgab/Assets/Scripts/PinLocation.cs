using UnityEngine;
using System.Collections;

public class PinLocation : MonoBehaviour {
    private Transform myTransform;
    public bool showTagDetail = false;
    private Vector3 screenPos;
    private float menuH,menuW,menuX,menuY;
    private Rect boxRect;
    public string tagString = "Lokasi";

    

	// Use this for initialization
	void Start () {
        myTransform = transform;
	}

    void OnMouseDown()
    {
        Debug.Log("PIN "+transform.name);
        showTagDetail = !showTagDetail;
    }

	// Update is called once per frame
	void Update () {
        //if (!showTagDetail) return;

        //screenPos = Camera.main.WorldToScreenPoint(myTransform.position);

        //menuH = 30f;
        //menuW = 300;
        //menuX = screenPos.x;
        //menuY = Screen.height - screenPos.y - menuH;
        //boxRect = new Rect(menuX, menuY, menuW, menuH);
	}

    //void OnGUI()
    //{
        //if (!showTagDetail) return;

        //GUI.backgroundColor = Color.yellow;
        //GUILayout.BeginArea(boxRect);
        //GUILayout.BeginVertical(GUI.skin.button,GUILayout.Width(50));
        //GUILayout.Label("Lokasi");
        //GUILayout.EndVertical();
        //GUILayout.EndArea();
    //}
}
