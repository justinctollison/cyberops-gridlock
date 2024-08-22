using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private BaseAction _selectedAction;
    //[SerializeField] private LayerMask unitsLayerMask;

    private bool _isBusy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"There's more than one UnitAction System! {transform} - {Instance}");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetSelectedUnit(_selectedUnit);
    }

    private void Update()
    {
        UnitSelectionAction();
        HandleSelectedAction();
    }

    // Unit Selection based on Mouse position, moved to MouseWorld.cs
    //private bool TryGetUnitSelection()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, unitsLayerMask))
    //    {
    //        if (raycastHit.collider.TryGetComponent(out Unit unit))
    //        {
    //            _selectedUnit = unit;
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //private void UnitMoveAction(MoveAction moveAction)
    //{
    //    GridPosition mouseGridposition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

    //    if (moveAction.IsValidActionGridPosition(mouseGridposition))
    //    {
    //        SetBusy();
    //        moveAction.TakeAction(mouseGridposition, ClearBusy);
    //    }
    //}

    //private void UnitSpinAction(SpinAction spinAction)
    //{
    //    SetBusy();
    //    spinAction.TakeAction(ClearBusy);
    //}

    private void HandleSelectedAction()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (_isBusy) { return; }
        if (!_selectedUnit) { return; }
        if (!TurnSystem.Instance.GetIsPlayerTurn()) { return; }

        // TODO: Fires at the same time as GetUnit and moves the unit at the same time if you select both a Unit and tile
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridposition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!_selectedAction.IsValidActionGridPosition(mouseGridposition)) { return; }
            if (!_selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction)) { return; }

            SetBusy();
            _selectedAction.TakeAction(mouseGridposition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }

        // Select Action with Switch Statement
        //if (Input.GetMouseButtonDown(0))
        //{
        //    switch (_selectedAction)
        //    {
        //        case MoveAction moveAction:
        //            UnitMoveAction(moveAction);
        //            break;
        //        case SpinAction spinAction:
        //            UnitSpinAction(spinAction);
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }

    private void SetBusy()
    {
        _isBusy = true;

        OnBusyChanged?.Invoke(this, _isBusy);
    }

    private void ClearBusy()
    {
        _isBusy = false;

        OnBusyChanged?.Invoke(this, _isBusy);
    }

    private void UnitSelectionAction()
    {
        if (!TurnSystem.Instance.GetIsPlayerTurn()) { return; }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetSelectedUnit(MouseWorld.GetUnit());
        }
    }

    private void SetSelectedUnit(Unit unit)
    {
        if (!unit) { return; }
        if (unit == _selectedUnit) { return; }

        _selectedUnit = unit;

        SetSelectedAction(unit.GetUnitMoveAction());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        _selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return _selectedAction;
    }
}
