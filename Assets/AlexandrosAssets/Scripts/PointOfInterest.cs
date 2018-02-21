using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.MeshGeneration.Factories;

public class PointOfInterest : MonoBehaviour {
	
	public DirectionsFactory directionsFactory;
	
	public List<MapManager.PoiCategory> categories;
	
	public float latitude;
	public float longitude;
	
	[Serializable]
	 public struct CategoryWeight {
		 public MapManager.PoiCategory category;
		 public int weight; //(0-10)
	 }
	 public CategoryWeight[] weightssda;

	// Use this for initialization
	void Start () {
		MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>();
		int polyCounter = 0;
		
		foreach (MeshFilter mesh in meshes){
			polyCounter += mesh.mesh.triangles.Length/3;
		}
		
		Debug.Log(polyCounter);
		
		weightssda = new CategoryWeight[8];
		weightssda[0].category = MapManager.PoiCategory.Museum;
		weightssda[1].category = MapManager.PoiCategory.Fortress;
		weightssda[2].category = MapManager.PoiCategory.Religion;
		weightssda[3].category = MapManager.PoiCategory.Lighthouse;
		weightssda[4].category = MapManager.PoiCategory.Plaza;
		weightssda[5].category = MapManager.PoiCategory.ArchaelogicalSite;
		weightssda[6].category = MapManager.PoiCategory.ArchitecturalMonument;
		weightssda[7].category = MapManager.PoiCategory.OldArtisanship;
		
	}
	
	void OnMouseDown()
	{
		directionsFactory.NewDestQuery(transform);
	}
	
}
