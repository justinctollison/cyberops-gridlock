using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject _actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera();
    }

    private void ShowActionCamera()
    {
        _actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        _actionCameraGameObject.SetActive(false);
    }

    public void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
                Vector3 shootDirection = (targetUnit.GetUnitWorldPosition() - shooterUnit.GetUnitWorldPosition()).normalized;

                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 9) * shootDirection * shoulderOffsetAmount;

                Vector3 actionCameraPosition = 
                    shooterUnit.GetUnitWorldPosition() + 
                    cameraCharacterHeight + 
                    shoulderOffset + 
                    (shootDirection * -1);

                _actionCameraGameObject.transform.position = actionCameraPosition;
                _actionCameraGameObject.transform.LookAt(targetUnit.GetUnitWorldPosition() + cameraCharacterHeight);

                ShowActionCamera();
                break;
            default:
                break;
        }
    }

    public void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch(sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
            default:
                break;
        }
    }
}
