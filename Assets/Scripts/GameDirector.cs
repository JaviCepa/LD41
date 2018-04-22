using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameDirector : SerializedMonoBehaviour
{

	public Dictionary<SpawnType, GameObject> spawnPrefabs;

	ZombieSpawner[] zombieSpawners;
	HumanSpawner[] humanSpawners;
	CrateSpawner[] crateSpawners;

	public AudioClip pickupWeaponClip;
	public GameObject healthBarPrefab;
	public HumanBase humanBase;

	public static GameDirector instance;

	public static int actorCount = 0;

	public int maxActorCount = 50;
	public float initialDelay = 5f;
	public float dayDuration = 120f;

	private void Awake()
	{
		instance = this;
		zombieSpawners = FindObjectsOfType<ZombieSpawner>();
		humanSpawners = FindObjectsOfType<HumanSpawner>();
		crateSpawners = FindObjectsOfType<CrateSpawner>();
		humanBase = FindObjectOfType<HumanBase>();
	}

	void Start()
	{
		StartCoroutine(RunningGame());
	}

	private IEnumerator RunningGame()
	{
		Time.timeScale = 0f;
		yield return new WaitUntil(() => Input.anyKeyDown);
		Time.timeScale = 1f;

		yield return new WaitForSeconds(initialDelay);
		GetComponent<AudioSource>().Play();
		int currentLevel = 1;
		do
		{
			Debug.Log("New level " + currentLevel);
			int humanCount = Random.Range(1, 3);
			for (int i = 0; i < humanCount; i++)
			{
				AddActor(SpawnType.Human);
			}
			int crateCount = Random.Range(2, 3);
			for (int i = 0; i < crateCount; i++)
			{
				AddActor(SpawnType.CrateBox);
			}
			yield return new WaitForSeconds(dayDuration * 0.5f);
			if (currentLevel > 4)
			{
				for (int i = 0; i < currentLevel - 4; i++)
				{
					AddActor(SpawnType.HugeZombie);
				}
			}
			if (currentLevel > 2)
			{
				for (int i = 0; i < (currentLevel - 1) * 2; i++)
				{
					AddActor(SpawnType.FatZombie);
				}
			}
			for (int i = 0; i < currentLevel * 3; i++)
			{
				AddActor(SpawnType.CommonZombie);
			}
			yield return new WaitForSeconds(dayDuration * 0.5f);
			currentLevel++;
		} while (true);
	}

	void AddActor(SpawnType actorType)
	{
		if (actorCount <= maxActorCount)
		{
			var position = GetSpawnPoint(actorType);
			Instantiate(spawnPrefabs[actorType], position, Quaternion.identity);
			actorCount++;
		}
	}

	public static void RemoveActor()
	{
		actorCount--;
	}

	private Vector3 GetSpawnPoint(SpawnType spawnType)
	{
		Vector3 result = Vector3.zero;
		if (spawnType.ToString().Contains("Zombie"))
		{
			result = zombieSpawners[Random.Range(0, zombieSpawners.Length)].GetSpawnPoint();
		}
		if (spawnType.ToString().Contains("Human"))
		{
			result = humanSpawners[Random.Range(0, humanSpawners.Length)].GetSpawnPoint();
		}
		if (spawnType.ToString().Contains("Crate"))
		{
			result = crateSpawners[Random.Range(0, crateSpawners.Length)].GetSpawnPoint();
		}
		return result;
	}
}

public enum SpawnType { None, Human, CommonZombie, FatZombie, HugeZombie, CrateBox }
