using UnityEngine;
using System.Collections;

public class GyroCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log (Input.gyro.enabled);

		Input.gyro.enabled = true;

		Debug.Log (Input.gyro.enabled);
	}
	
	void Update () 
	{
		GetComponent<Camera>().transform.Rotate (0, -Input.gyro.rotationRateUnbiased.y, 0);

		//Debug.Log (Input.gyro.rotationRateUnbiased.y);
	}
	void OnGUI() {
		if (GUI.Button(new Rect(10, 10, 150, 100), Input.gyro.enabled.ToString()))
			print("You clicked the button!");

	}

}
