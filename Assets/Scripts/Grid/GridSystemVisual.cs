using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform _gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

    public static GridSystemVisual Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"There's more than one GridSystemVisual System! {transform} - {Instance}");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CreateGridSystemVisual();
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void CreateGridSystemVisual()
    {
        _gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetGridWidth(),
            LevelGrid.Instance.GetGridHeight()
        ];

        for (int x = 0; x < LevelGrid.Instance.GetGridWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetGridHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform gridSystemVisualSingleTransform =
                    Instantiate(_gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                _gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
    }

    public void HideAllGridPositions()
    {
        foreach (var gridSystemVisualSingle in _gridSystemVisualSingleArray)
        {
            gridSystemVisualSingle.GetComponent<GridSystemVisualSingle>().Hide();
        }
    }

    public void ShowGridPositionsList(List<GridPosition> gridPositionsList)
    {
        foreach (GridPosition gridPosition in gridPositionsList)
        {
            _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPositions();

        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        if (selectedAction == null) { return; }

        ShowGridPositionsList(selectedAction.GetValidActionGridPositionList());
    }
}
