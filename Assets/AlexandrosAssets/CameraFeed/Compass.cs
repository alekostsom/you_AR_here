using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Input.compass.enabled = true;
		//Input.location.Start ();

		StartCoroutine (getCompass ());
	}
	float tmp;
	// Update is called once per frame
	void Update () {
		tmp = Input.compass.trueHeading;

		GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -tmp);
	}

	IEnumerator getCompass(){
		yield return new WaitForSeconds (0.5f);



		StartCoroutine (getCompass ());
	}
}
