using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputController), typeof(PlayerInfoModel))]
public class PlayerDashInfoModel : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] private GameObject prefabDashEffect;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    
    private bool canDash = true;
    private bool isDashed = false;

    private PlayerInputController inputController;
    private PlayerInfoModel infoModel;

    private void Start()
    {
        infoModel = GetComponent<PlayerInfoModel>();
        inputController = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
        Dash();
        infoModel.IsDashing = isDashed;
    }

    private void Dash()
    {
        if (inputController.IsDownLeftShift && canDash && !isDashed)
            StartCoroutine(DashCalculate());
    }

    private IEnumerator DashCalculate()
    {
        isDashed = true;
        canDash = false;
        infoModel.SetGravityScale(0);
        infoModel.SetRigidbodyVelocity(new Vector2(transform.localScale.x * dashSpeed, 0));
        if(infoModel.OnGround) Instantiate(prefabDashEffect, transform);
        yield return new WaitForSeconds(dashTime);
        infoModel.ResetGravityScale();
        isDashed = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}