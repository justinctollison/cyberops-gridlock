using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] private bool _isEnemy;

    private Animator _animator;
    private MoveAction _unitMoveAction;
    private SpinAction _unitSpinAction;
    private BaseAction[] _baseActionArray;

    private HealthSystem _healthSystem;
    private GridPosition _gridPosition;

    [SerializeField] private int _actionPoints = ACTION_POINTS_MAX;

    private void Awake()
    {
        _unitMoveAction = GetComponent<MoveAction>();
        _unitSpinAction = GetComponent<SpinAction>();
        _baseActionArray = GetComponents<BaseAction>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        _healthSystem.OnDead += HealthSystem_OnDead;
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
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }

    public MoveAction GetUnitMoveAction()
    {
        return _unitMoveAction;
    }

    public SpinAction GetSpinAction()
    {
        return _unitSpinAction;
    }

    public GridPosition GetUnitGridPosition()
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
    }

    public void Damage(int damageAmount)
    {
        _healthSystem.Damage(damageAmount);

        Debug.Log($"{gameObject.name} at {transform.position} damaged!");
    }
}
