using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnNumberText;
    [SerializeField] private Button _turnButton;
    [SerializeField] private GameObject _enemyTurnVisual;

    private void Start()
    {
        _turnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnVisibility();
    }

    private void UpdateTurnText()
    {
        _turnNumberText.text = "Turn: " + TurnSystem.Instance.GetTurnNumber();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnVisibility();
    }

    private void UpdateEnemyTurnVisual()
    {
        _enemyTurnVisual.SetActive(!TurnSystem.Instance.GetIsPlayerTurn());
    }

    private void UpdateEndTurnVisibility()
    {
        _turnButton.gameObject.SetActive(TurnSystem.Instance.GetIsPlayerTurn());
    }
}
