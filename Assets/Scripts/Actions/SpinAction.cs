using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;

    private void Update()
    {
        if (!_isActive) { return; }

        Spin();
    }

    public void Spin()
    {
        float spinAddAmount = 360f * Time.deltaTime;

        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        _totalSpinAmount += spinAddAmount;
        if (_totalSpinAmount >= 360)
        {
            ActionComplete();
        }
    }

    public  override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _totalSpinAmount = 0;

        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = _unit.GetGridPosition();

        return new List<GridPosition> { unitGridPosition };
    }

    public override bool GetIsActive()
    {
        return _isActive;
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }
}
