using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float _timer;

    private enum State { WaitingForEnemyTurn, TakingTurn, Busy }
    private State _state;

    private void Awake()
    {
        _state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.GetIsPlayerTurn()) { return; }

        switch (_state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                EndTurnTimer();

                break;
            case State.Busy:
                break;
            default:
                break;
        }
    }

    private void EndTurnTimer()
    {
        _timer -= Time.deltaTime;

        if (_timer < 0)
        {
            if (TryTakeEnemyAIAction(SetStateTakingTurn))
            {
                _state = State.Busy;
            }
            else
            {
                // No more enemies have actions they can take, end enemy turn
                TurnSystem.Instance.NextTurn();
            }
        }
    }

    private void SetStateTakingTurn()
    {
        _timer = 0.5f;
        _state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.GetIsPlayerTurn()) { return; }

        _state = State.TakingTurn;
        _timer = 2f;
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {
                // Enemy cannot afford this action
                continue;
            }

            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAIAction != null && CompareActions(bestEnemyAIAction, testEnemyAIAction))
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CompareActions(EnemyAIAction bestBaseAction, EnemyAIAction testEnemyAIAction)
    {
        if (bestBaseAction.actionValue > testEnemyAIAction.actionValue) return false; //Do not replace
        if (bestBaseAction.actionValue == testEnemyAIAction.actionValue) return UnityEngine.Random.Range(0, 100) < 50 ? true : false;
        return true;
    }
}
