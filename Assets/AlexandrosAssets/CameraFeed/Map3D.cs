using UnityEngine;
using System.Collections;

public class Map3D : MonoBehaviour {
	// x refers to longitude, y refers to latitude
	public Vector2 centerPoint = new Vector2(19.91242f, 39.61817f);
	public Vector2 topLeftCorner = new Vector2 (19.86575f, 39.65410f);
	public Vector2 bottomRightCorner = new Vector2 (19.95909f, 39.58223f);

	public Transform topLeftMarker;
	public Transform bottomRightMarker;

	public Transform myPosition;
	public Transform camPos;
	void Start(){
		//Debug.Log (GeoCoordsTo3dMapXY(new Vector2(19.92950f ,39.6172f)));

		//myPosition.position = GeoCoordsTo3dMapXY (new Vector2 (19.924153f, 39.625677f));

		StartCoroutine (StartLocationServices ());
	}

	public Vector3 GeoCoordsTo3dMapXY(Vector2 geoCoords){
		float x = (float)((-topLeftCorner.x + geoCoords.x) * transform.localScale.x) / (float)(bottomRightCorner.x - topLeftCorner.x);
		float z = (float)((geoCoords.y - topLeftCorner.y) * transform.localScale.z) / (float)( bottomRightCorner.y - topLeftCorner.y);

		return new Vector3 (topLeftMarker.position.x + x, 0, topLeftMarker.position.z - z);
	}

	public Vector3 GeoCoordsTo3dMapGPS(){
		float x = (float)((-topLeftCorner.x + Input.location.lastData.longitude) * transform.localScale.x) / (float)(bottomRightCorner.x - topLeftCorner.x);
		float z = (float)((Input.location.lastData.latitude - topLeftCorner.y) * transform.localScale.z) / (float)( bottomRightCorner.y - topLeftCorner.y);

		return new Vector3 (topLeftMarker.position.x + x, /*Input.location.lastData.altitude*/ 2.0f, topLeftMarker.position.z - z);
	}

	public void Update(){
		//print ("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
		camPos.transform.position = GeoCoordsTo3dMapGPS();
	}

	public IEnumerator StartLocationServices(){
		// First, check if user has location service enabled
		////if (Input.location.isEnabledByUser)
			//yield break;

		// Start service before querying location
		Input.location.Start ();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds (1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			print ("Timed out");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			print ("Unable to determine device location");
			yield break;
		} else {
			// Access granted and location value could be retrieved
			print ("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
		}

		// Stop service if there is no need to query location updates continuously
		//Input.location.Stop ();
	}

	void OnGUI(){
		GUI.Box (new Rect (10, 400, 200, 100), camPos.transform.position.ToString());
		GUI.Box (new Rect (10, 500, 200, 100), "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude);
	}
}
