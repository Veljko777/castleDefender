using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public Tile[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float tileRadius { get; private set; }
    public Tile destinationTile;

    private float tileDiameter;

    public FlowField(float _cellRadius, Vector2Int _gridSize)
    {
        tileRadius = _cellRadius;
        tileDiameter = tileRadius * 2f;
        gridSize = _gridSize;
    }

    public void CreateGrid()
    {
        grid = GameObject.Find("GameLogic").GetComponent<LevelGenerator>().grid;
    }

    public void CreateCostField()
    {
        foreach (Tile curCell in grid)
        {
            if (curCell.isWall)
            {
                curCell.IncreaseCost(255);
            }
        }
    }

    public void CreateIntegrationField(Tile _destinationCell)
    {
        destinationTile = _destinationCell;
        destinationTile.cost = 0;
        destinationTile.bestCost = 0;
        Queue<Tile> tilesToCheck = new Queue<Tile>();
        tilesToCheck.Enqueue(destinationTile);

        while (tilesToCheck.Count > 0)
        {
            Tile curTile = tilesToCheck.Dequeue();
            List<Tile> curNeighbors = GetNeighborCells(curTile.gridIndex, GridDirection.CardinalDirections);
            foreach (Tile curNeighbor in curNeighbors)
            {
                if (curNeighbor.cost == byte.MaxValue) { continue; }
                if (curNeighbor.cost + curTile.bestCost < curNeighbor.bestCost)
                {
                    curNeighbor.bestCost = (ushort)(curNeighbor.cost + curTile.bestCost);
                    tilesToCheck.Enqueue(curNeighbor);
                }
            }
        }
    }

    public void CreateFlowField()
    {
        foreach (Tile curTile in grid)
        {
            List<Tile> curNeighbors = GetNeighborCells(curTile.gridIndex, GridDirection.AllDirections);

            int bestCost = curTile.bestCost;

            foreach (Tile curNeighbor in curNeighbors)
            {
                if (curNeighbor.bestCost < bestCost)
                {
                    bestCost = curNeighbor.bestCost;
                    curTile.bestDirection = GridDirection.GetDirectionFromV2I(curNeighbor.gridIndex - curTile.gridIndex);
                }
            }
        }
    }

    private List<Tile> GetNeighborCells(Vector2Int nodeIndex, List<GridDirection> directions)
    {
        List<Tile> neighborTile = new List<Tile>();

        foreach (Vector2Int curDirection in directions)
        {
            Tile newNeighbor = GetCellAtRelativePos(nodeIndex, curDirection);
            if (newNeighbor != null)
            {
                neighborTile.Add(newNeighbor);
            }
        }
        return neighborTile;
    }

    private Tile GetCellAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
    {
        Vector2Int finalPos = orignPos + relativePos;

        if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
        {
            return null;
        }
        else { return grid[finalPos.x, finalPos.y]; }
    }

    public Tile GetCellFromWorldPos(Vector3 worldPos)
    {
        float percentX = worldPos.x / (gridSize.x * tileDiameter);
        float percentY = worldPos.y / (gridSize.y * tileDiameter);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
        return grid[x, y];
    }
}