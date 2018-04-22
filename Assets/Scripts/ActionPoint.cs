using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPoint : MonoBehaviour
{

	public bool isAvailable { get { return GetComponentInChildren<Human>() == null; } }

	private void OnTriggerEnter(Collider other)
	{
		var player = other.GetComponent<HumanPlayer>();
		if (player != null)
		{
			player.AssignFollower(this);
		}
	}

}
