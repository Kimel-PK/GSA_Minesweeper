using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public int x;
	public int z;
	public bool isMined;
	public bool isChecked;
	public int minedNeighbours;
	public Minefield minefield;
	public GameObject tileGO;
	public GameObject checkedTilePrefab;
	public GameObject flagGO;
	public GameObject flagPrefab;
	public ParticleSystem mud;

	private bool isFlagged;

	public void Dig()
	{
		if (isMined)
		{
			// TODO game over logic goes here
			Debug.Log("Game Over!");
			return;
		}
		
		if (isChecked || isFlagged) // can't dig again digged tile or dig flagged tile
			return;
		
		Destroy(tileGO);
		tileGO = Instantiate(checkedTilePrefab, transform.position, Quaternion.Euler(Vector3.zero), transform);
		mud.Play();
		isChecked = true;
		// if there is no mines around this tile all adjacent tiles will be automatically digged
		if (minedNeighbours == 0)
			minefield.DigAllNeighbours(x, z);
	}
	
	public void ToggleFlag () {
		// tile was digged already, return
		if (isChecked)
			return;
		
		// if isFlagged destroy flag prefab, if is not create new flag prefab
		if (isFlagged)
			Destroy(flagGO);
		else
			flagGO = Instantiate(flagPrefab, transform.position, Quaternion.Euler(Vector3.zero), transform);
		
		// toggle isFlagged bool
		isFlagged = !isFlagged;
	}
}