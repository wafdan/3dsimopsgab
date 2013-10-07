using UnityEngine;
using System.Collections;

public class FlashPoint : MonoBehaviour {

	public float flashRate = 1.0f;
	private Color originalColor;
	//private UnitManager unitManager;
	
	void Start() {
		GameObject unitManagerObject = GameObject.FindGameObjectWithTag("point");
		//unitManager = unitManagerObject.GetComponent<UnitManager>();
		originalColor = renderer.material.color;
		StartCoroutine("Flash");
	}
	
	void Update() {
		/* if(unitManager.IsSelected(gameObject)) {
			StartCoroutine("Flash");
		}
		else {
			StopAllCoroutines();
			renderer.material.color = originalColor;
		} */
	}
	
	IEnumerator Flash() {
		float t = 0;
		while(t < flashRate) {
			renderer.material.color = Color.Lerp(originalColor, Color.green, t/flashRate);
			t += Time.deltaTime;
			yield return null;
		}
		renderer.material.color = Color.green;
		StartCoroutine("Return");
	}
	
	IEnumerator Return() {
		float t = 0;
		while(t < flashRate) {
			renderer.material.color = Color.Lerp(originalColor, Color.green, t/flashRate);
			t += Time.deltaTime;
			yield return null;
		}
		renderer.material.color = Color.green;
		StartCoroutine("Flash");
	}
}
