using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : Interactable
{
	public GameObject newGameObject;
	public Transform spawnPoint;

	public void InstantiateNewObject()
	{
		Instantiate(newGameObject,spawnPoint.position,Quaternion.identity);
	}
}
