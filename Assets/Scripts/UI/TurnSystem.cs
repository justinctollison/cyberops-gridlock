using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    private int _turnNumber = 1;
    private bool _isPlayerTurn = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"There's more than one Turn System! {transform} - {Instance}");
            Destroy(gameObject);
        }
    }

    public void NextTurn()
    {
        _turnNumber++;
        _isPlayerTurn = !_isPlayerTurn;

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return _turnNumber;
    }

    public bool GetIsPlayerTurn()
    {
        return _isPlayerTurn;
    }
}
