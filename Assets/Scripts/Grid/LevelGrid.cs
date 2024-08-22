using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform _gridDebugObjectPrefab;
    private GridSystem _gridSystem;

    public static LevelGrid Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"There's more than one LevelGrid System! {transform} - {Instance}");
            Destroy(gameObject);
        }

        _gridSystem = new GridSystem(10, 10, 2f);
        DebugObjectSpawn();
    }

    private void DebugObjectSpawn()
    {
        _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        _gridSystem.GetGridObject(gridPosition).AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        return _gridSystem.GetGridObject(gridPosition).GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        _gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public int GetGridHeight() => _gridSystem.GetGridHeight();

    public int GetGridWidth() => _gridSystem.GetGridWidth();

}
