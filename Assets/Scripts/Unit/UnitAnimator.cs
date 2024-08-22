using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _bulletSmokeVFXPrefab;
    [SerializeField] private Transform _bulletProjectilePrefab;
    [SerializeField] private Transform _shootPointTransform;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        
        if (TryGetComponent<SpinAction>(out SpinAction spinAction))
        {

        }
        
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        _animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = Instantiate(_bulletProjectilePrefab, _shootPointTransform.position, Quaternion.identity);
        Instantiate(_bulletSmokeVFXPrefab, _shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetUnitWorldPosition();
        targetUnitShootAtPosition.y = _shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        _animator.SetBool("IsWalking", true);
    }
    
    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        _animator.SetBool("IsWalking", false);
    }

}
