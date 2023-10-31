using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
	[Header("Params")]
	[SerializeField]
	private float _duration;
	
	[Header("Components")]
	[SerializeField]
	private GameObject _helpGO;
	[SerializeField]
	private GameObject _winGO;
	[SerializeField]
	private GameObject _loseGO;
	[SerializeField]
	private TextMeshProUGUI _timeText;
	//-----------------------------------------------------------
	//-----------------------------------------------------------
	private EState                 _state;
	private float                  _endTime;
	private List<BrickPlaceholder> _bricksPlaceholders;
	private bool                   _isWinCountdownOn;

	private void Start()
	{
		Application.targetFrameRate = 60;
		
		_endTime = Time.time + _duration;

		_bricksPlaceholders = new List<BrickPlaceholder>(
			FindObjectsOfType<BrickPlaceholder>(true)
		);

		_state         = EState.Help;
		_helpGO.SetActive(true);
		Time.timeScale = 0;

		_timeText.text = $"Осталось {_duration:F0} сек";
	}

	private void Update()
	{
		if (_state == EState.Game)
		{
			if (Input.GetKeyDown(KeyCode.F1))
			{
				_helpGO.SetActive(true);
				Time.timeScale = 0;

				_state = EState.Help;
			}

			if (IsWinning() && !_isWinCountdownOn)
				StartCoroutine(CrWinCountdown());
			
			float secsLeft = _endTime - Time.time;
			if (secsLeft < 0)
			{
				_loseGO.SetActive(true);
				Time.timeScale = 0;
				_state         = EState.Lose;
			}
			_timeText.text = $"Осталось: {secsLeft:F0} сек";
		}
		else if (_state == EState.Help)
		{
			if (Input.GetKeyDown(KeyCode.F1))
			{
				_helpGO.SetActive(false);
				Time.timeScale = 1;
				_state         = EState.Game;
			}
		}
		else if (_state is EState.Win or EState.Lose)
		{
			if (Input.GetKeyDown(KeyCode.R))
				SceneManager.LoadScene(sceneBuildIndex: 0, LoadSceneMode.Single);
		}
	}


	private bool IsWinning()
	{
		foreach (var ph in _bricksPlaceholders)
			if (!ph.IsFilled)
				return false;
		return true;
	}

	private IEnumerator CrWinCountdown()
	{
		_isWinCountdownOn = true;

		for (var i = 0; i < 4; i++)
		{
			yield return new WaitForSeconds(1.0f);
			if (!IsWinning())
			{
				_isWinCountdownOn = false;
				yield break;
			}
		}

		yield return new WaitWhile(() => _state != EState.Game);
		
		_state = EState.Win;
		_winGO.SetActive(true);
		Time.timeScale = 0;
	}


	private enum EState
	{
		Game,
		Help,
		Win,
		Lose
	}
}