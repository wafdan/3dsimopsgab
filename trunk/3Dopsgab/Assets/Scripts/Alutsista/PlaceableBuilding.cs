using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceableBuilding : MonoBehaviour {
	
	[HideInInspector]
	public List<Collider> colliders = new List<Collider>();
	private bool isSelected;
	public string bName;
    private BasicUnitMovement bm;//caching

	void OnGUI() {
		if (isSelected) {
			GUI.Button(new Rect(Screen.width /2, Screen.height / 20, 100, 30), bName);	
		}
		
	}
	
	void OnTriggerEnter(Collider c) {
		if (c.tag == "Building" || IsKenaDaratan(c)) {
			colliders.Add(c);	
		}
	}
	
	void OnTriggerExit(Collider c) {
        if (c.tag == "Building" || IsKenaDaratan(c))
        {
			colliders.Remove(c);	
		}
	}
	
	public void SetSelected(bool s) {
		isSelected = s;	
	}

    public bool IsKenaDaratan(Collider c)
    {
        bm = (bm==null)?gameObject.GetComponent<BasicUnitMovement>():bm;
        if (bm.isUnitLaut)
        {
            if (c.tag == "daratan")
            {
                return true;
            }
        }
        return false;
    }
}
