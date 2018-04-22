using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FxManager : MonoBehaviour {

	public List<GameObject> effects;

	public static FxManager instance;

	public float bitsExplosion = 10f;

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

	[Button("TestZombieBits")]
	public void TestZombieBits()
	{
		SpawnZombieBits(transform.position, 5);
	}

	public static void SpawnZombieBits(Vector3 position, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Debug.Log(i);
			var itemToSpawn = instance.fxDictionary["ZombieBits"];
			var newObject = Instantiate(itemToSpawn, position+Random.insideUnitSphere*0.5f, Random.rotationUniform) as GameObject;
			newObject.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * Random.value * instance.bitsExplosion);
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
