using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    List<Unit> _unitList = new List<Unit>();
    List<Unit> _friendlyUnitList = new List<Unit>();
    List<Unit> _enemyUnitList = new List<Unit>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"There's more than one UNitManager System! {transform} - {Instance}");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        if (unit.GetIsEnemy())
        {
            _enemyUnitList.Add(unit);
        }
        else
        {
            _friendlyUnitList.Add(unit);
        }

        _unitList.Add(unit);
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        if (unit.GetIsEnemy())
        {
            _enemyUnitList.Remove(unit);
        }
        else
        {
            _friendlyUnitList.Remove(unit);
        }

        _unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return _unitList;
    }
    public List<Unit> GetFriendlyUnitList()
    {
        return _friendlyUnitList;
    }
    public List<Unit> GetEnemyUnitList()
    {
        return _enemyUnitList;
    }
}
