using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FxManager : MonoBehaviour {

	public List<GameObject> effects;

	public static FxManager instance;

	Dictionary<string, GameObject> fxDictionary;
	
	void Awake()
	{
		instance = this;
		fxDictionary = new Dictionary<string, GameObject>();
		foreach (var effect in effects)
		{
			fxDictionary.Add(effect.name, effect);
		}
	}
	
	public static void DisplayFx(string fxName, Vector3 position, float duration = 5f)
	{
		if (instance.fxDictionary.ContainsKey(fxName))
		{
			var itemToSpawn = instance.fxDictionary[fxName];
			var newObject = Instantiate(itemToSpawn, position, itemToSpawn.transform.rotation);
			Destroy(newObject, duration);
		}
		else
		{
			Debug.LogWarning("Fx with name '" + fxName + "' was not found in the FxManager");
		}
	}
}
