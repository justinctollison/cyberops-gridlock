using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro _gridObjectText;

    private object _gridObject;

    protected virtual void Update()
    {
        _gridObjectText.text = _gridObject.ToString();
    }

    public virtual void SetGridObject(object gridObject)
    {
        _gridObject = gridObject;
    }
}
