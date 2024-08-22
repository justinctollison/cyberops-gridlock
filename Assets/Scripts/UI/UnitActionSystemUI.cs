using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainer;
    [SerializeField] private TextMeshProUGUI _actionPointsText;

    private List<ActionButtonUI> _actionButtonUIList;

    private void Awake()
    {
        _actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        UpdateActionPoints();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void CreateUnitActionButtons()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == null) { return; }

        foreach (Transform buttonPrefab in _actionButtonContainer)
        {
            Destroy(buttonPrefab.gameObject);
        }

        _actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(_actionButtonPrefab, _actionButtonContainer);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            _actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in _actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == null) { return; }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        _actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
    }
    
}
