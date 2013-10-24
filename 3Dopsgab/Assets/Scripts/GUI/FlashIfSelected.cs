using UnityEngine;
using System.Collections;

public class FlashIfSelected : MonoBehaviour {

	public float flashRate = 1.0f;
	private Color originalColor;
    private Color flashColor;
	private UnitManager unitManager;
	
	void Start() {
		GameObject unitManagerObject = GameObject.FindGameObjectWithTag("unitmanager");
		unitManager = unitManagerObject.GetComponent<UnitManager>();
		originalColor = renderer.material.color;
        flashColor = Color.white;
		//StartCoroutine("Flash");
	}
	
	void Update() {
        if (!BuildingPlacement.hasPlaced) return;
		if(unitManager.IsSelected(gameObject)) {
			//StartCoroutine("Flash");
            renderer.material.color = flashColor;
		}
		else {
			//StopAllCoroutines();
			renderer.material.color = originalColor;
		}
	}
	
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
}
