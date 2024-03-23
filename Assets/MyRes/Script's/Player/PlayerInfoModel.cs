using System.Linq;
using UnityEngine;
using System.Collections;
using System;



/// <summary>
/// Скрипт отвечает за контроль движения сущности. 
/// </summary>
[RequireComponent(typeof(PlayerInputController))]
public class PlayerInfoModel : MonoBehaviour
{
    public enum PlayerLookingTo
    {
        LookingLeft,
        LookingRight
    }

    [Header("Speed")]
    [SerializeField] private float walkSpeed;
    [Space(8)]

    [Header("Length of the ground check ray")]
    [SerializeField] private float groundCheckYRayDistance = 0.2f;
    [SerializeField] private float groundCheckXRayDistance = 0.5f;

    [Header("Ground")]
    [SerializeField] private Transform groundCheckEntity;
    [SerializeField] private LayerMask groundLayer;

    private PlayerInputController inputController;
    public float gravity;

    public bool OnGround {  get; private set; }
    public bool IsWalking { get; private set; }
    public bool IsJumping;
    public bool IsDashing;
    public bool IsAttacking;


    [Header("Debug")]
    private Rigidbody2D rigidbody;
    public PlayerLookingTo lookingTo;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        inputController = GetComponent<PlayerInputController>();

        gravity = rigidbody.gravityScale;
    }

    void Update()
    {
        if (IsDashing) return;
        UpdateLookingTo();
        CheckGround();

        Move();
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
            rigidbody.velocity = new Vector2(walkSpeed * inputController.AxisX, rigidbody.velocity.y);
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

    private void UpdateLookingTo()
    {
        if (inputController.AxisX > 0)
            lookingTo = PlayerLookingTo.LookingRight;

        if (inputController.AxisX < 0)
            lookingTo = PlayerLookingTo.LookingLeft;
    }

    public void SetGravityScale(float setGravity)
    {
        rigidbody.gravityScale = setGravity;
    }

    public void ResetGravityScale()
    {
        rigidbody.gravityScale = gravity;
    }

    public void SetRigidbodyVelocity(Vector2 vector2)
    {
        rigidbody.velocity = vector2;
    }

    public Vector2 GetRigitbodyVelocity()
    {
        return rigidbody.velocity;
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