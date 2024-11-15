using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private bool _isEnemy;

    private BaseAction[] _baseActionArray;

    private HealthSystem _healthSystem;
    private GridPosition _gridPosition;

    [SerializeField] private int _actionPoints = ACTION_POINTS_MAX;

    private void Awake()
    {
        _baseActionArray = GetComponents<BaseAction>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        _healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        CheckGridPosition();
    }

    public void CheckGridPosition()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (_gridPosition != newGridPosition)
        {
            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;


            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in _baseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }

        return null;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public Vector3 GetUnitWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return _baseActionArray;
    }

    public int GetActionPoints()
    {
        return _actionPoints;
    }

    public bool GetIsEnemy()
    {
        return _isEnemy;
    }

    public float GetHealthNormalized()
    {
        return _healthSystem.GetHealthNormalized();
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (_actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;

        Debug.Log($"Action Points are being spent, removing AP: {_actionPoints} by Cost: {amount}");

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((GetIsEnemy() && !TurnSystem.Instance.GetIsPlayerTurn()) ||
            (!GetIsEnemy() && TurnSystem.Instance.GetIsPlayerTurn()))
        {
            _actionPoints = ACTION_POINTS_MAX;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);

        Destroy(gameObject);
        
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public void Damage(int damageAmount)
    {
        _healthSystem.Damage(damageAmount);

        Debug.Log($"{gameObject.name} at {transform.position} damaged!");
    }
}
