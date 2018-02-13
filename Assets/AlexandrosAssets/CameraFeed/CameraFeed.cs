using UnityEngine;
using System.Collections;

public class CameraFeed : MonoBehaviour {

	public WebCamTexture mCamera = null;
	public GameObject plane;

	// Use this for initialization
	void Start ()
	{
		StartFeeding();

	}

	// Update is called once per frame
	void Update ()
	{

	}
	
	public void StartFeeding(){
		Debug.Log ("Script has been started");
		//plane = GameObject.FindWithTag ("Player");

		mCamera = new WebCamTexture ();
		plane.GetComponent<Renderer>().material.mainTexture = mCamera;
		mCamera.Play ();
		Debug.Log(mCamera);
	}
}
