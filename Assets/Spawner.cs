using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{

	public float spawnRadius = 5f;
	public Color spawnerColor = Color.white;

	private void OnDrawGizmos()
	{
		Gizmos.color = spawnerColor;
		Gizmos.DrawWireSphere(transform.position, spawnRadius);
	}

	public Vector3 GetSpawnPoint()
	{
		var radial = Random.insideUnitCircle * Random.value * spawnRadius;
		return transform.position + new Vector3(radial.x, 0, radial.y);
	}
	
}
