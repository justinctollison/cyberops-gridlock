using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField] private LayerMask mousePlaneLayerMask;
    [SerializeField] private LayerMask unitsLayerMask;

    private static MouseWorld instance;

    private void Awake()
    {
        if (this != null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, instance.mousePlaneLayerMask);
        return raycastHit.point;
    }

    public static Unit GetUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, instance.unitsLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit) && !unit.GetIsEnemy())
            {
                return unit;
            }
        }

        return null;
    }
}
