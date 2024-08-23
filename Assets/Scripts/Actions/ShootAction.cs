using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State { Aiming, Shooting, CoolOff }
    private State _state;

    [SerializeField] private Unit _targetUnit;

    private int _maxShootDistance = 7;
    private float _stateTimer;
    private bool _canShootBullet;

    private void Update()
    {
        if (!_isActive) { return; }

        _stateTimer -= Time.deltaTime;

        switch (_state)
        {
            case State.Aiming:
                Vector3 aimDirection = (_targetUnit.GetUnitWorldPosition() - _unit.GetUnitWorldPosition()).normalized;

                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (_canShootBullet)
                {
                    _canShootBullet = false;
                    Shoot();
                }
                break;
            case State.CoolOff:
                break;
            default:
                break;
        }

        if (_stateTimer <= 0)
        {
            NextState();
        }
    }
    
    private void NextState()
    {
        switch (_state)
        {
            case State.Aiming:
                _state = State.Shooting;
                float shootingStateTimer = 0.1f;
                _stateTimer = shootingStateTimer;
                break;
            case State.Shooting:
                _state = State.CoolOff;
                float coolOffStateTimer = 0.5f;
                _stateTimer = coolOffStateTimer;
                break;
            case State.CoolOff:
                ActionComplete();
                break;
            default:
                break;
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = _targetUnit,
            shootingUnit = _unit
        });
        
        _targetUnit.Damage(40);
    }

    public Vector3 GetTargetPosition()
    {
        return _targetUnit.transform.position;
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override bool GetIsActive()
    {
        return _isActive;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetUnitGridPosition();

        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                };

                int shootDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (shootDistance > _maxShootDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unity occupying it
                    continue;
                }

                Unit target = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (target.GetIsEnemy() == _unit.GetIsEnemy())
                {
                    // Both units on same Team
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        _state = State.Aiming;
        float aimingStateTime = 1f;
        _stateTimer = aimingStateTime;

        _canShootBullet = true;
    }
}
