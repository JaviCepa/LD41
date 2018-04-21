using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHider : MonoBehaviour {

	private void OnTriggerEnter(Collider other)
	{
		//other.SendMessageUpwards("HideBuilding", SendMessageOptions.DontRequireReceiver);
	}

	private void OnTriggerExit(Collider other)
	{
		//other.SendMessageUpwards("ShowBuilding", SendMessageOptions.DontRequireReceiver);
	}
	
}
