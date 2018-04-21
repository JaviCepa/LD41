using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameDirector : SerializedMonoBehaviour
{

	public Dictionary<SpawnType, GameObject> spawnPrefabs;

	public List<EnemyWave> waves;

	ZombieSpawner[] zombieSpawners;
	HumanSpawner[] humanSpawners;

	int currentWaveIndex = 0;

	private void Awake()
	{
		zombieSpawners = FindObjectsOfType<ZombieSpawner>();
		humanSpawners = FindObjectsOfType<HumanSpawner>();
	}

	[Button("Spawn next wave")]
	public void SpawnNextWave()
	{
		Debug.Log("Spawning wave: " + currentWaveIndex);
		SpawnWave(waves[currentWaveIndex]);
		currentWaveIndex++;
	}

	void SpawnWave(EnemyWave wave)
	{
		foreach (var spawn in wave.spawns)
		{
			for (int i = 0; i < spawn.amount; i++)
			{
				var targetPosition = GetSpawnPoint(spawn.type);
				Instantiate(spawnPrefabs[spawn.type], targetPosition, Quaternion.identity);
			}
		}
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
		return result;
	}
}

[System.Serializable]
public class EnemyWave
{
	public List<WaveSpawn> spawns;
}

[System.Serializable]
public class WaveSpawn
{
	[HorizontalGroup("WaveSpawn")]
	public int amount;
	[HorizontalGroup("WaveSpawn")]
	public float spawnTime = 0;
	[EnumToggleButtons]public SpawnType type;
}

public enum SpawnType { None, Human, CommonZombie, FatZombie, HugeZombie }
