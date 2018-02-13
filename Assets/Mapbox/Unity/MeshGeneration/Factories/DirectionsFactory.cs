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

	public class DirectionsFactory : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		MeshModifier[] MeshModifiers;

		[SerializeField]
		Transform[] _waypoints;

		[SerializeField]
		Material _material;

		[SerializeField]
		float _directionsLineWidth;
		
		private Directions _directions;
		private int _counter;

		GameObject _directionsGO;
		
		[SerializeField]
		GameObject go_pointSphere;
		List<GameObject> pointsList = new List<GameObject>();
		
		public GameObject go_poiUI;
		
		public Text gpsText;
		
		public GameObject navCamera, feedCamera;
		
		void Awake()
		{
			if (_map == null)
			{
				_map = FindObjectOfType<AbstractMap>();
			}
			_directions = MapboxAccess.Instance.Directions;
			//_map.OnInitialized += Query;
		}

		void OnDestroy()
		{
			_map.OnInitialized -= Query;
		}

		void Query()
		{
			var count = _waypoints.Length;
			var wp = new Vector2d[count];
			for (int i = 0; i < count; i++)
			{
				wp[i] = _waypoints[i].GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
			}
			var _directionResource = new DirectionResource(wp, RoutingProfile.Walking);
			_directionResource.Steps = true;
			_directionResource.Alternatives = true;
			_directions.Query(_directionResource, HandleDirectionsResponse);
		}

		void HandleDirectionsResponse(DirectionsResponse response)
		{
			if (null == response.Routes || response.Routes.Count < 1)
			{
				return;
			}
			
			Debug.Log("Code: " + response.Code);
			Debug.Log("Route Distance: " + response.Routes[0].Distance);
			Debug.Log("Route Duration: " + response.Routes[0].Duration);
			Debug.Log("Routes: " + response.Routes.Count);
			Debug.Log("Waypoints: " + response.Waypoints.Count);
			
			go_poiUI.SetActive(true);
			
			foreach(GameObject go in pointsList){
				Destroy(go);
			}

			var meshData = new MeshData();
			var dat = new List<Vector3>();
			foreach (var point in response.Routes[0].Geometry)
			{
				Debug.Log ("x: " + point.x + ", y: " + point.y);
				dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
			}
			
			/*var meshData2 = new MeshData();
			var dat2 = new List<Vector3>();
			foreach (var point in response.Routes[1].Geometry)
			{
				Debug.Log ("x: " + point.x + ", y: " + point.y);
				dat2.Add(Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
			}*/
			
			foreach (var d in dat)
			{
				GameObject point_GO = GameObject.Instantiate(go_pointSphere, transform, false) as GameObject;
				
				point_GO.transform.localPosition = d;//new Vector3((float)point.x, 1.0f, (float)point.y);
				
				pointsList.Add(point_GO);
			}

			var feat = new VectorFeatureUnity();
			feat.Points.Add(dat);

			foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
			{
				var lineMod = mod as LineMeshModifier;
				if (lineMod != null)
				{
					lineMod.Width = _directionsLineWidth / _map.WorldRelativeScale;
				}
				mod.Run(feat, meshData, _map.WorldRelativeScale);
			}

			CreateGameObject(meshData);
			
			/*foreach (var d in dat2)
			{
				GameObject point_GO = GameObject.Instantiate(_pointSphere, transform, false) as GameObject;
				
				point_GO.transform.localPosition = d;//new Vector3((float)point.x, 1.0f, (float)point.y);
			}

			var feat2 = new VectorFeatureUnity();
			feat2.Points.Add(dat2);

			foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
			{
				var lineMod = mod as LineMeshModifier;
				if (lineMod != null)
				{
					lineMod.Width = _directionsLineWidth / _map.WorldRelativeScale;
				}
				mod.Run(feat2, meshData2, _map.WorldRelativeScale);
			}

			CreateGameObject(meshData2);*/
		}

		GameObject CreateGameObject(MeshData data)
		{
			if (_directionsGO != null)
			{
				Destroy(_directionsGO);
			}
			_directionsGO = new GameObject("direction waypoint " + " entity");
			var mesh = _directionsGO.AddComponent<MeshFilter>().mesh;
			mesh.subMeshCount = data.Triangles.Count;

			mesh.SetVertices(data.Vertices);
			_counter = data.Triangles.Count;
			for (int i = 0; i < _counter; i++)
			{
				var triangle = data.Triangles[i];
				mesh.SetTriangles(triangle, i);
			}

			_counter = data.UV.Count;
			for (int i = 0; i < _counter; i++)
			{
				var uv = data.UV[i];
				mesh.SetUVs(i, uv);
			}

			mesh.RecalculateNormals();
			_directionsGO.AddComponent<MeshRenderer>().material = _material;
			return _directionsGO;
		}
		
		// Alexandros
		public void NewDestQuery(Transform newDest){
			_waypoints[1].localPosition = newDest.localPosition;
			//_map.OnInitialized += Query;
			Query();
		}
		
		public void InitialiseNavigation(){
			
			GameObject go_NavEntity = GameObject.Instantiate(_directionsGO, GameObject.Find("NavigationParent").transform, false) as GameObject;
			//GameObject go_startPos = GameObject.Instantiate(_waypoints[0], GameObject.Find("NavigationParent").transform, false) as GameObject;
			navCamera.SetActive(true);
			navCamera.transform.position = _waypoints[0].position;
			navCamera.transform.Translate(Vector3.up * 1.8f);
			
			
			
			foreach(GameObject go in pointsList){
				go.transform.SetParent(GameObject.Find("NavigationParent").transform);
			}
			
			GameObject.Find("direction waypoint  entity").transform.SetParent(GameObject.Find("NavigationParent").transform);
			
			// Start GPS services (probably handled before)
			StartCoroutine(navCamera.GetComponent<GPS_Transform>().StartLocationServices());
			
			// Move user's view to GPS location
			
			
			// Stop looking for Vuforia imageTarget	and enable simple device camera
			GameObject.Find("ARCamera").SetActive(false);
			feedCamera.SetActive(true);
			
		}		
		
	}

}