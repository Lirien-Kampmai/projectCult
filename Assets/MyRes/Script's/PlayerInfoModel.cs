using System.Linq;
using UnityEngine;
using System.Collections;
using System;



/// <summary>
/// Скрипт отвечает за контроль движения сущности. 
/// </summary>
public class PlayerInfoModel : MonoBehaviour
{

    public enum PlayerState
    {
        Idle,
        Walking,
        Jumping,
        Dashing,
        Attacking
    }

    public enum PlayerLookingTo
    {
        LookingLeft,
        LookingRight
    }

    [SerializeField] PlayerInputController inputController;
    [SerializeField] PlayerViewController viewController;

    [Header("Speed")]
    [SerializeField] private float walkSpeed;
    [Space(8)]

    [Header("Dash")]
    [SerializeField] private GameObject prefabDashEffect;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    private bool canDash = true;
    private bool isDashed;

    [Header("Length of the ground check ray")]
    [SerializeField] private float groundCheckYRayDistance = 0.2f;
    [SerializeField] private float groundCheckXRayDistance = 0.5f;

    [Header("Ground")]
    [SerializeField] private Transform groundCheckEntity;
    [SerializeField] private LayerMask groundLayer;


    private bool onGround;
    public bool OnGround => onGround;

    [Header("Debug")]
    public Vector2 moveDir;
    public new Rigidbody2D rigidbody;
    public float gravity;
    public PlayerState playerState;
    public PlayerLookingTo lookingTo;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        gravity = rigidbody.gravityScale;
    }

    void Update()
    {
        if (playerState == PlayerState.Dashing) return;
        UpdateLookingTo();
        CheckGround();

        Move();
        Dash();

    }

    private void Move()
    {
        if (inputController.AxisX == 0)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            playerState = PlayerState.Idle;
        }
        else
        {
            rigidbody.velocity = new Vector2(walkSpeed * inputController.AxisX, rigidbody.velocity.y);
            playerState = PlayerState.Walking;
        }
    }

    #region Dash
    private void Dash()
    {
        if(inputController.IsDownLeftShift  && canDash && !isDashed)
        {
            StartCoroutine(DashCalculate());
            isDashed = true;
        }

        if(onGround) isDashed = false;
    }

    private IEnumerator DashCalculate()
    {
        canDash = false;
        playerState = PlayerState.Dashing;
        rigidbody.gravityScale = 0;
        rigidbody.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        if (onGround) Instantiate(prefabDashEffect, transform);
        yield return new WaitForSeconds(dashTime);
        rigidbody.gravityScale = gravity;
        playerState = PlayerState.Idle;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    #endregion


    private void CheckGround()
    {
        if (Physics2D.Raycast(groundCheckEntity.position, Vector2.down, groundCheckYRayDistance, groundLayer)
            || Physics2D.Raycast(groundCheckEntity.position + new Vector3( groundCheckXRayDistance, 0, 0), Vector2.down, groundCheckYRayDistance, groundLayer) 
            || Physics2D.Raycast(groundCheckEntity.position + new Vector3(-groundCheckXRayDistance, 0, 0), Vector2.down, groundCheckYRayDistance, groundLayer))
            onGround = true;
        else
            onGround = false;
    }

    private void UpdateLookingTo()
    {
        if (inputController.AxisX > 0)
            lookingTo = PlayerLookingTo.LookingRight;
        else
            lookingTo = PlayerLookingTo.LookingLeft;
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