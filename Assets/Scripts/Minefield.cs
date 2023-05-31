using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Minefield : MonoBehaviour
{
	public GameObject tilePrefab;
	public GameObject playerPrefab;
	public Tile[,] minefield;

	private void Start()
	{
        Instantiate(playerPrefab, new Vector3(0, 0, -(GameManager.Singleton.sizeZ-1)/2f - 3 ), Quaternion.identity); // Create player.
		GenerateMinefield(GameManager.Singleton.sizeX, GameManager.Singleton.sizeZ, GameManager.Singleton.minesCount);
	}

	public void GenerateMinefield(int sizeX, int sizeZ, int minesCount)
	{
		minefield = new Tile[sizeX, sizeZ];

        // Offsets used to place the minefiled in the middle of the scene.
        float offsetX = (sizeX-1) / 2f;
        float offsetZ = (sizeZ-1) / 2f;

        // create tiles instances
		for (int x = 0; x < sizeX; x++)
		{
			for (int z = 0; z < sizeZ; z++)
			{
                // create new tile prefab instance
				GameObject tmpTile = Instantiate(tilePrefab, new Vector3(x - offsetX, 0, z - offsetZ), Quaternion.Euler(Vector3.zero), transform);
                // get Tile component and store it in array
				minefield[x, z] = tmpTile.GetComponent<Tile>();
                // tile uses reference to this script when calling DigAllNeighbours() method
                minefield[x, z].minefield = this;
                minefield[x, z].x = x;
                minefield[x, z].z = z;
			}
		}

        // plant mines
        for (int m = 0; m < minesCount;)
		{
            // choose random spot on minefield
            int x = Random.Range(0, sizeX);
            int z = Random.Range(0, sizeZ);
            // get Tile object from array
			Tile tmp = minefield[x, z];
            // if tile is mined go back and try again
			if (tmp.isMined)
				continue;
            
			tmp.isMined = true;
            
            // each tile stores number of mined neighbours to tell detector how many beeps this tile would make
            // we must go trough all adjacent tiles and increment minedNeighbours variable
            for (int i = x - 1; i <= x + 1; i++)
            {
                // if we go out of bounds go back to start of loop
                if (i < 0 || i >= sizeX)
                    continue;

                for (int j = z - 1; j <= z + 1; j++)
                {
                    // if we go out of bounds or checking center tile go back to start of loop
                    if (j < 0 || j >= sizeZ || (i == x && j == z))
                        continue;

                    minefield[i, j].minedNeighbours++;
                }
            }
            
			m++;
		}
	}
    
    public IEnumerator DigAllNeighbours (int x, int z)
    {
	    int sizeX = GameManager.Singleton.sizeX;
	    int sizeZ = GameManager.Singleton.sizeZ;
	    
        // go trough all neighbours like in mines generation and call Dig() method on them
        for (int i = x - 1; i <= x + 1; i++)
        {
            if (i < 0 || i >= sizeX)
                continue;

            for (int j = z - 1; j <= z + 1; j++)
            {
                if (j < 0 || j >= sizeZ || (i == x && j == z))
                    continue;

                yield return new WaitForSeconds(0.2f);
                minefield[i, j].Dig();
            }
        }
    }
}