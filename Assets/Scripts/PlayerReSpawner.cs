using System;
using System.Collections;
using HeneGames.Airplane;
using UnityEngine;

public class PlayerReSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject _prefPlayer;

	//-----------------------------------------------------------
	//-----------------------------------------------------------
	private SimpleAirPlaneController _controller;

	private bool _isReSpawning;
	
	private void Start()
	{
		Spawn();
	}

	private void Update()
	{
		if (_isReSpawning)
			return;
		if (_controller.PlaneIsDead())
		{
			StartCoroutine(CrReSpawn());
		}
	}

	private IEnumerator CrReSpawn()
	{
		_isReSpawning = true;

		yield return new WaitForSeconds(1f);
		Destroy(_controller.gameObject);
		Spawn();
		
		_isReSpawning = false;
	}

	private void Spawn()
	{
		var go = Instantiate(_prefPlayer, transform.position, Quaternion.identity);
		_controller = go.GetComponent<SimpleAirPlaneController>();
	}
}