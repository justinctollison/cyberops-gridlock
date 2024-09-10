using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform _ragDollPrefab;
    [SerializeField] private Transform _originalRootBone;

    private HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();

        _healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragDollTransform = Instantiate(_ragDollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragDollTransform.GetComponent<UnitRagdoll>();

        unitRagdoll.Setup(_originalRootBone);
    }
}
