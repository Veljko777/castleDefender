using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float tileRadius = 0.5f;
    public FlowField curFlowField;
    //enable for debug
    //public GridDebug gridDebug;

    public void SetDestination()
    {
        InitializeFlowField();
        curFlowField.CreateCostField();
        Tile destinationCell = curFlowField.GetCellFromWorldPos(GameObject.Find("Base").transform.position);
        curFlowField.CreateIntegrationField(destinationCell);
        curFlowField.CreateFlowField();
    }

    private void InitializeFlowField()
    {
        curFlowField = new FlowField(tileRadius, gridSize);
        curFlowField.CreateGrid();
        //enable for debug
        //gridDebug.SetFlowField(curFlowField);
    }
}

