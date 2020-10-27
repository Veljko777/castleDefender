using UnityEditor;
using UnityEngine;

public enum FlowFieldDisplayType { None, AllIcons, DestinationIcon, CostField, IntegrationField };

public class GridDebug : MonoBehaviour
{
    public GridController gridController;
    public bool displayGrid;

    public FlowFieldDisplayType curDisplayType;

    private Vector2Int gridSize;
    private float tileRadius;
    private FlowField curFlowField;


    public void SetFlowField(FlowField newFlowField)
    {
        curFlowField = newFlowField;
        tileRadius = newFlowField.tileRadius;
        gridSize = newFlowField.gridSize;
    }


    private void DisplayCell(Tile tile)
    {
        GameObject iconGO = new GameObject();
        SpriteRenderer iconSR = iconGO.AddComponent<SpriteRenderer>();
        iconGO.transform.parent = transform;
        iconGO.transform.position = tile.worldPos;
    }

    public void ClearCellDisplay()
    {
        foreach (Transform t in transform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (displayGrid)
        {
            if (curFlowField == null)
            {
                DrawGrid(gridController.gridSize, Color.yellow, gridController.tileRadius);
            }
            else
            {
                DrawGrid(gridSize, Color.green, tileRadius);
            }
        }

        if (curFlowField == null) { return; }

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        //switch (curDisplayType)
        //{
        //    case FlowFieldDisplayType.CostField:

        //        foreach (Tile curTile in curFlowField.grid)
        //        {
        //            Handles.Label(curTile.worldPos, curTile.cost.ToString(), style);
        //        }
        //        break;

        //    case FlowFieldDisplayType.IntegrationField:

        //        foreach (Tile curCell in curFlowField.grid)
        //        {
        //            Handles.Label(curCell.worldPos, curCell.bestCost.ToString(), style);
        //        }
        //        break;

        //    default:
        //        break;
        //}
    }

    private void DrawGrid(Vector2Int drawGridSize, Color drawColor, float drawCellRadius)
    {
        Gizmos.color = drawColor;
        for (int x = 0; x < drawGridSize.x; x++)
        {
            for (int y = 0; y < drawGridSize.y; y++)
            {
                Vector3 center = new Vector3(drawCellRadius * 2 * x + drawCellRadius, drawCellRadius * 2 * y + drawCellRadius,1);
                Vector3 size = Vector3.one * drawCellRadius * 2;
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}