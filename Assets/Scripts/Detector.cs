using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
	AudioSource audioPlayer;
	Coroutine beepingCoroutine;
	public Transform raycastSource;
	int _detectedMines;
	int DetectedMines
	{
		get
		{
			// if we want get detected mines value just return it
			return _detectedMines;
		}
		set
		{
			// if we want to set new value
			
			// first check if new value is different than old
			if (value == _detectedMines)
				return;
			
			// set new value of detectedMines
			_detectedMines = value;
			
			// if there is no mines nearby
			if (value == 0)
			{
				// if beeping coroutine is playing then stop it
				if (beepingCoroutine != null)
					StopCoroutine(beepingCoroutine);
			}
			else
			{
				// if beeping coroutine is playing stop it
				if (beepingCoroutine != null)
					StopCoroutine(beepingCoroutine);
				// start new coroutine with proper number of beeps
				beepingCoroutine = StartCoroutine(Beep());
			}
		}
	}

	void Awake()
	{
		audioPlayer = GetComponent<AudioSource>();
	}

	void Update()
	{
		Detect();
	}

	void Detect()
	{
		// draw a ray in Scene view
		Debug.DrawRay(raycastSource.position, raycastSource.forward, Color.red, 0.1f);
		
		// cast a ray to check if it hits object with tag "Tile"
		RaycastHit rHit;
		if (!Physics.Raycast(raycastSource.position, raycastSource.forward, out rHit, 2f) || !rHit.collider.CompareTag ("Tile"))
		{
			// if there is no Tile object set detected mines to 0 and return
			DetectedMines = 0;
			return;
		}

		// if Tile was hit then get information of mined neighbours from that Tile
		Tile pointedTile = rHit.collider.GetComponent<Tile>();
		DetectedMines = pointedTile.minedNeighbours;
	}

	IEnumerator Beep()
	{
		while (true)
		{
			// play beep for every detected mine
			for (int i = 0; i < DetectedMines; i++)
			{
				audioPlayer.Play();
				yield return new WaitForSeconds(0.1f);
			}
			// then wait for 1 second and start again
			yield return new WaitForSeconds(1f);
		}
	}
}
