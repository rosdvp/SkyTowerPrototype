using System;
using System.Collections.Generic;
using HeneGames.Airplane;
using UnityEngine;

public class BrickPicker : MonoBehaviour
{
	[Header("Params")]
	[SerializeField]
	private Vector3 _pickedBoxPos;
	[SerializeField]
	private float _dropPower;

	[Header("Components")]
	[SerializeField]
	private Transform _planeTf;
	
	//-----------------------------------------------------------
	//-----------------------------------------------------------
	private List<Transform> _nearBoxesTf = new();
	
	private Transform _pickedBoxTf;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (_pickedBoxTf == null)
			{
				if (_nearBoxesTf.Count == 0)
					return;

				var currPos = transform.position;

				var   nearestTf   = _nearBoxesTf[0];
				float nearestDist = Vector3.Distance(currPos, _nearBoxesTf[0].position);
				foreach (var tf in _nearBoxesTf)
				{
					float dist = Vector3.Distance(currPos, tf.position);
					if (dist < nearestDist)
					{
						nearestDist = dist;
						nearestTf   = tf;
					}
				}

				_pickedBoxTf               = nearestTf;
				_pickedBoxTf.parent        = transform;
				_pickedBoxTf.localPosition = _pickedBoxPos;
				_pickedBoxTf.localRotation = Quaternion.Euler(0, 0, 0);

				_pickedBoxTf.GetComponent<Rigidbody>().isKinematic = true;

				_nearBoxesTf.Remove(nearestTf);
			}
			else
			{
				DropBrick();
			}
		}
	}

	private void DropBrick()
	{
		_pickedBoxTf.parent = null;
				
		var rb = _pickedBoxTf.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.AddForce(_planeTf.forward * _dropPower, ForceMode.VelocityChange);

		_pickedBoxTf = null;
	}
    
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log($"in {other.transform.parent.gameObject.name}");
		
		_nearBoxesTf.Add(other.transform.parent);
	}

	private void OnTriggerExit(Collider other)
	{
		Debug.Log($"out {other.gameObject.name}");
		
		_nearBoxesTf.Remove(other.transform.parent);
	}
	
	private void OnDestroy()
	{
		if (_pickedBoxTf != null)
			DropBrick();
	}
}
