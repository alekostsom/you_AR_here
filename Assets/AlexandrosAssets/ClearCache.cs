using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Mapbox.Map;
using Mapbox.Unity;

public class ClearCache : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MapboxAccess.Instance.ClearCache();		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
