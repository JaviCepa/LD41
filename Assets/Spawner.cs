using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

	public float spawnRadius = 5f;
	public Color spawnerColor = Color.white;

	private void OnDrawGizmos()
	{
		Gizmos.color = spawnerColor;
		Gizmos.DrawWireSphere(transform.position, spawnRadius);
	}
	
}
