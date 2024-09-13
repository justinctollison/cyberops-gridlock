using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    private Vector3 _targetPosition;

    [SerializeField] private int _maxMoveDistance = 4;
    [SerializeField] private float _movementSpeed = 4f;
 
    protected override void Awake()
    {
        base.Awake();

        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (!_isActive) { return; }

        Move();
    }

    public override void TakeAction(GridPosition position, Action onActionComplete)
    {
        _targetPosition = LevelGrid.Instance.GetWorldPosition(position);

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public void Move()
    {
        float distance = Vector3.Distance(transform.position, _targetPosition);
        float stoppingDistance = 0.1f;
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;

        if (distance > stoppingDistance)
        {
            transform.position += moveDirection * _movementSpeed * Time.deltaTime;
        }
        else
        {
            ActionComplete();
            OnStopMoving?.Invoke(this, EventArgs.Empty);
        }

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    // Moved to BaseAction class
    //public bool IsValidActionGridPosition(GridPosition gridPosition)
    //{
    //    List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
    //    return validGridPositionList.Contains(gridPosition);
    //}

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                };

                if (unitGridPosition == testGridPosition)
                {
                    // Grid Position already occupied by this same Unit.
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position already occupied by another Unit.
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override bool GetIsActive()
    {
        return _isActive;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = _unit.GetShootAction().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}
