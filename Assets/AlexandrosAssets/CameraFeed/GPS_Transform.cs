namespace Mapbox.Unity.MeshGeneration.Factories
{
	using System.Collections;
	using UnityEngine;
	using UnityEngine.UI;
	using Mapbox.Directions;
	using System.Collections.Generic;
	using System.Linq;
	using Mapbox.Unity.Map;
	using Data;
	using Modifiers;
	using Mapbox.Utils;
	using Mapbox.Unity.Utilities;
	
	public class GPS_Transform : MonoBehaviour {
		
			public Text gpsText;
			AbstractMap _map;
		// Use this for initialization
		void Awake()
		{
			if (_map == null)
			{
				_map = FindObjectOfType<AbstractMap>();
			}
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		public IEnumerator StartLocationServices(){
			
			// First, check if user has location service enabled
			//if (Input.location.isEnabledByUser)
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
				gpsText.text = "Timed out" ;
				yield break;
			}

			// Connection has failed
			if (Input.location.status == LocationServiceStatus.Failed) {
				gpsText.text = "Unable to determine device location";
				yield break;
			} else {
				// Access granted and location value could be retrieved
				gpsText.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy;
				StartCoroutine(UpdateCameraPos());
			}

			// Stop service if there is no need to query location updates continuously
			//Input.location.Stop ();
		}
		
		IEnumerator UpdateCameraPos(){
			yield return new WaitForSeconds(0.1f);
			
			//transform.position = Conversions.GeoToWorldPosition(39.62566, 19.92422, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();
			transform.position = Conversions.GeoToWorldPosition(Input.location.lastData.latitude, Input.location.lastData.longitude, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz() + Vector3.up * 1.8f;
			//transform.position = new Vector3(gpsPos.x, 1.8f, gpsPos.y);
		
			StartCoroutine(UpdateCameraPos());
		}
	}
}
