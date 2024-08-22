using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro _gridObjectText;

    private GridObject _gridObject;

    private void Update()
    {
        _gridObjectText.text = _gridObject.ToString();
    }

    public void SetGridObject(GridObject gridObject)
    {
        _gridObject = gridObject;
    }
}
