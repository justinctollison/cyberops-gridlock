using System.Collections.Generic;

public class GridObject
{
    private List<Unit> _unitList;

    public GridSystem gridSystem;
    public GridPosition gridPosition;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        _unitList = new List<Unit>();
    }

    public void AddUnit(Unit unit)
    {
        this._unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        this._unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return _unitList;
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in _unitList)
        {
            unitString += unit + "\n";
        }
        return gridPosition.ToString() + "\n" + unitString;
    }

    public bool HasAnyUnit()
    {
        return _unitList.Count > 0;
    }

    public Unit GetUnit()
    {
        if (!HasAnyUnit()) { return null; }

        return _unitList[0];
    }
}
