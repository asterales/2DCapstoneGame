﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public bool pathable;
    public TileLocation position;
    public SelectionController selectionController; // hack for movement
    private TileStats tileStats;
    public Unit currentUnit;
    public GameObject movementTile;

    public void Awake() {
        tileStats = this.gameObject.GetComponent<TileStats>();
        position = this.gameObject.GetComponent<TileLocation>();
        ////// DEBUG CODE //////
        if (tileStats == null)
        {
            Debug.Log("Error :: Object Must Have TileStats Object -> Tile.cs");
        }

        if (position == null)
        {
            Debug.Log("Error :: Object Must Have TileLocation Object -> Tile.cs");
        }
        ////////////////////////
    }

    public void Update()
    {
    }

    public void OnMouseDown()
    {
        ClearMovementTiles();
        selectionController.clickedTile = this;
        if (currentUnit)
            ShowMovementTiles(currentUnit.unitStats.speed+1);
    }

    public void ShowMovementTiles(int speed)
    {
        Queue<Tile> toCheck = new Queue<Tile>();
        Queue<int> dist = new Queue<int>();
        toCheck.Enqueue(this);
        dist.Enqueue(speed);
        List<Tile> neighbors;
        while (toCheck.Count > 0)
        {
            Tile t = toCheck.Dequeue();
            Vector3 pos = t.movementTile.transform.position;
            speed = dist.Dequeue();
            if (pos.z > 0 && speed>0)
            {
                t.movementTile.transform.position = new Vector3(pos.x, pos.y, -pos.z);
                neighbors = t.GetNeighbors();
                foreach (Tile neighbor in neighbors)
                {
                    toCheck.Enqueue(neighbor);
                    dist.Enqueue(speed - 1);
                }
            }
        }
    }

    public List<Tile> GetNeighbors()
    {
        List<Tile> neighbors = new List<Tile>();
        try
        {
            neighbors.Add(HexMap.mapArray[position.row + 1][position.col]);
        } catch (System.ArgumentOutOfRangeException e) { };
        try
        {
            neighbors.Add(HexMap.mapArray[position.row - 1][position.col]);
        }
        catch (System.ArgumentOutOfRangeException e) { };
        try
        {
            neighbors.Add(HexMap.mapArray[position.row][position.col - 1]);
        }
        catch (System.ArgumentOutOfRangeException e) { };
        try
        {
            neighbors.Add(HexMap.mapArray[position.row][position.col + 1]);
        }
        catch (System.ArgumentOutOfRangeException e) { };
        try
        {
            neighbors.Add(HexMap.mapArray[position.row + 1][position.col + 1]);
        }
        catch (System.ArgumentOutOfRangeException e) { };
        try
        {
            neighbors.Add(HexMap.mapArray[position.row - 1][position.col - 1]);
        }
        catch (System.ArgumentOutOfRangeException e) { };
        return neighbors;

    }

    public void ClearMovementTiles()
    {
        foreach (List<Tile> row in HexMap.mapArray)
            foreach (Tile t in row)
                if (t.movementTile.transform.position.z < 0)
                    t.movementTile.transform.position = new Vector3(t.movementTile.transform.position.x, t.movementTile.transform.position.y, -t.movementTile.transform.position.z);
    }
}
