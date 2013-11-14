using UnityEngine;
using System.Collections;

public class FlashIfSelected : MonoBehaviour {

	public float flashRate = 1.0f;
    private Transform myTransform;
	private Color originalColor;
    private Color flashColor;
	private UnitManager unitManager;
	
	void Start() {
		GameObject unitManagerObject = GameObject.FindGameObjectWithTag("unitmanager");
		unitManager = unitManagerObject.GetComponent<UnitManager>();
		originalColor = getOriginalColor();
        
        //MeshRenderer mr = GetComponent<MeshRenderer>();
        //Debug.Log("ORIG COLOR OF " + gameObject.name + " : " + originalColor+". amount: "+mr.materials.Length);
        flashColor = Color.green;
        myTransform = transform;
		//StartCoroutine("Flash");
	}

    private Color getOriginalColor()
    {
        Color color = (renderer.material.HasProperty("_Color")) ? renderer.material.color : (new Color(0.419f, 0.419f, 0.419f, 1.000f)); //mencegah error: Material doesn't have a color property '_Color'
        BasicUnitMovement bm = GetComponent<BasicUnitMovement>();
        if (bm != null)
        {
            if (bm.isUnitLaut)
            {
                color = new Color(0.419f, 0.419f, 0.419f, 1.000f);
            }
        }
        return color;
    }
	
	void Update() {
        if (!BuildingPlacement.hasPlaced) return;
		if(unitManager.IsSelected(gameObject)) {
			//StartCoroutine("Flash");
            gameObject.renderer.material.color = flashColor;
            if (myTransform.childCount > 0)
            {
                //MeshRenderer[] rends = myTransform.GetComponentsInChildren<MeshRenderer>();
                //for (int i = 0; i < rends.Length; i++)
                //{
                //    rends[i].material.color = Color.green;
                //}
                colorTheChilds(myTransform,Color.green);
            }
		}
		else {
			//StopAllCoroutines();
			gameObject.renderer.material.color = originalColor;
            if (myTransform.childCount > 0)
            {
                //MeshRenderer[] rends = myTransform.GetComponentsInChildren<MeshRenderer>();
                //for (int i = 0; i < rends.Length; i++)
                //{
                //    rends[i].material.color = originalColor;
                //}
                colorTheChilds(myTransform, originalColor);
            }
		}
	}

    private void colorTheChilds(Transform curTransform, Color color)
    {
        if (curTransform.childCount > 0)
        {
            Renderer[] co = curTransform.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < co.Length; i++)
            {
                co[i].material.color = color;
                //colorTheChilds(co[i].transform, color);
            }
        }
    }

	/*
	IEnumerator Flash() {
		float t = 0;
		while(t < flashRate) {
            renderer.material.color = Color.Lerp(originalColor, flashColor, t / flashRate);
			t += Time.deltaTime;
			yield return null;
		}
        renderer.material.color = flashColor;
		StartCoroutine("Return");
	}
	
	IEnumerator Return() {
		float t = 0;
		while(t < flashRate) {
            renderer.material.color = Color.Lerp(originalColor, flashColor, t / flashRate);
			t += Time.deltaTime;
			yield return null;
		}
        renderer.material.color = flashColor;
		StartCoroutine("Flash");
	}
    */
}
