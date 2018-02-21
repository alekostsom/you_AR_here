using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
	
	public static MapManager mapManagerInstance;
	
	// Poi category
	public enum PoiCategory{
		Museum,
		Fortress,
		Religion,
		Lighthouse,
		Plaza,
		ArchaelogicalSite,
		ArchitecturalMonument,
		OldArtisanship
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
