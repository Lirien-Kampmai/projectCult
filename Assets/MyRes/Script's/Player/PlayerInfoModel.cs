using System.Linq;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;



/// <summary>
/// Скрипт отвечает за контроль движения сущности. 
/// </summary>
[RequireComponent(typeof(PlayerInputController))]
public class PlayerInfoModel : Entity
{
    public enum PlayerLookingTo
    {
        LookingLeft,
        LookingRight
    }

    [Header("Length of the ground check ray")]
    [SerializeField] private float groundCheckYRayDistance = 0.2f;
    [SerializeField] private float groundCheckXRayDistance = 0.5f;

    [Header("Ground")]
    [SerializeField] private Transform groundCheckEntity;
    [SerializeField] private LayerMask groundLayer;


    private PlayerInputController inputController;
    private AttackLogic attackLogic;


    

    [Header("Debug")]

    private float impactRecoilTimer;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        
        inputController = GetComponent<PlayerInputController>();
        attackLogic = GetComponent<AttackLogic>();

        gravity = rigidbody.gravityScale;
    }

    void Update()
    {
        if (IsDashing) return;
        CheckGround();

        Move();
        AttackCheck();

        UpdateLookingTo();
        ImpactRecoiling();
    }

    private void AttackCheck()
    {
        if (inputController.IsLeftMouseButtonDown)
            attackLogic.Attack();
    }

    private void ImpactRecoiling()
    {
        if (!isImpactRecoiling) return;

        if (impactRecoilTimer < impactRecoilLenght)
        {
            impactRecoilTimer += Time.deltaTime;
        }
        else
        {
            isImpactRecoiling = false;
            impactRecoilTimer = 0;
        }
    }

    private void Move()
    {
        if (inputController.AxisX == 0)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            IsWalking = false;
        }
        else
        {
            rigidbody.velocity = new Vector2(speed * inputController.AxisX, rigidbody.velocity.y);
            IsWalking = true;
        }
    }

    private void CheckGround()
    {
        if (Physics2D.Raycast(groundCheckEntity.position, Vector2.down, groundCheckYRayDistance, groundLayer)
            || Physics2D.Raycast(groundCheckEntity.position + new Vector3( groundCheckXRayDistance, 0, 0), Vector2.down, groundCheckYRayDistance, groundLayer) 
            || Physics2D.Raycast(groundCheckEntity.position + new Vector3(-groundCheckXRayDistance, 0, 0), Vector2.down, groundCheckYRayDistance, groundLayer))
            OnGround = true;
        else
            OnGround = false;
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GizmosRayToGround();
    }
    private void GizmosRayToGround()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(groundCheckEntity.position, new Vector3(groundCheckEntity.position.x, groundCheckEntity.position.y - groundCheckYRayDistance, 0));
        Gizmos.DrawLine(groundCheckEntity.position, new Vector3(groundCheckEntity.position.x + groundCheckXRayDistance, groundCheckEntity.position.y, 0));
        Gizmos.DrawLine(groundCheckEntity.position, new Vector3(groundCheckEntity.position.x - groundCheckXRayDistance, groundCheckEntity.position.y, 0));
    }
#endif
}