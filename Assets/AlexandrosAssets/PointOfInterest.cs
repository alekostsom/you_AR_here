using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.MeshGeneration.Factories;

public class PointOfInterest : MonoBehaviour {
	
	public DirectionsFactory directionsFactory;

	// Use this for initialization
	void Start () {
		MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>();
		int polyCounter = 0;
		
		foreach (MeshFilter mesh in meshes){
			polyCounter += mesh.mesh.triangles.Length/3;
		}
		
		Debug.Log(polyCounter);
		
	}
	
	void OnMouseDown()
	{
		directionsFactory.NewDestQuery(transform);
	}
	
}
