using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPlaceholder : MonoBehaviour
{
	[Header("Params")]
	[SerializeField]
	private Color _colorEmpty;
	[SerializeField]
	private Color _colorFilled;
	
	[Header("Components")]
	[SerializeField]
	private Outline _outline;

	//-----------------------------------------------------------
	//-----------------------------------------------------------
	private int _counter;

	private void Start()
	{
		UpdateColor();
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Brick"))
		{
			_counter += 1;
			UpdateColor();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Brick"))
		{
			_counter -= 1;
			UpdateColor();
		}
	}

	public bool IsFilled => _counter > 0;

	private void UpdateColor()
	{
		_outline.OutlineColor = _counter > 0 ? _colorFilled : _colorEmpty;
	}
}
