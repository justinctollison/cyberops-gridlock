using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer;

    [SerializeField] private Transform _bulletHitVFXPrefab;

    private Vector3 _targetPosition;

    public void Setup(Vector3 position)
    {
        _targetPosition = position;
    }

    private void Update()
    {
        ShootProjectile();
    }

    private void ShootProjectile()
    {
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);

        float moveSpeed = 200f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float distanceAfterMoaving = Vector3.Distance(transform.position, _targetPosition);

        if (distanceBeforeMoving < distanceAfterMoaving)
        {
            transform.position = _targetPosition;

            _trailRenderer.transform.parent = null;

            Destroy(gameObject);

            Instantiate(_bulletHitVFXPrefab, _targetPosition, Quaternion.identity);
        }
    }
}
