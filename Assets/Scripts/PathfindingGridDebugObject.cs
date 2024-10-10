using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro _gCostText;
    [SerializeField] private TextMeshPro _fCostText;
    [SerializeField] private TextMeshPro _hCostText;

    private PathNode _pathNode;

    protected override void Update()
    {
        base.Update();

        _gCostText.text = _pathNode.GetGCost().ToString();
        _fCostText.text = _pathNode.GetFCost().ToString();
        _hCostText.text = _pathNode.GetHCost().ToString();
    }

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);

        _pathNode = (PathNode)gridObject;
    }
}
