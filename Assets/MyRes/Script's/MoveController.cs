using System.Linq;
using UnityEngine;
using System.Collections;

/// <summary>
/// Скрипт отвечает за контроль движения сущности. 
/// </summary>
public class MoveController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float walkSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private int jumpBufferFrame;
    [SerializeField] private int maxAirJump; // recomended value 1
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float coyoteTime = 0.3f; // recomended value ~0.3/ any low value

    [Header("Dash")]
    [SerializeField] private GameObject prefabDashEffect;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;

    [Header("Length of the ground check ray")]
    [SerializeField] private float groundCheckYRayDistance = 0.2f;
    [SerializeField] private float groundCheckXRayDistance = 0.5f;

    [Header("Ground")]
    [SerializeField] private Transform groundCheckEntity;
    [SerializeField] private LayerMask groundLayer;

    private CharacterStateList characterState;
    private Rigidbody2D rigidbody;
    private Animator animator;

    private int jumpBufferCounter;
    private int airJumpCounter = 0;
    private float axisHorizontal;
    private float coyoteTimeCounter;
    private float gravity;

    private bool canDash = true;
    private bool isDashed;

    [Header("Debug")]
    public Vector2 moveDir;

    private void Start()
    {
        characterState = GetComponent<CharacterStateList>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator  = GetComponent<Animator>();

        gravity = rigidbody.gravityScale;
    }

    void Update()
    {
        if (characterState.GetStateIsDashing()) return;

        Flip();
        Move();
        Jump();
        StartDash();
    }

    #region Move
    private void Move()
    {
        GetAxis();
        SetHorizontalVelocity();
        SetWalkAnimation();
    }
    private void GetAxis()
    {
        axisHorizontal = Input.GetAxisRaw("Horizontal");
    }

    private void SetHorizontalVelocity()
    {
        rigidbody.velocity = new Vector2(walkSpeed * axisHorizontal, rigidbody.velocity.y);
    }

    private void SetWalkAnimation()
    {
        animator.SetBool("Walking", rigidbody.velocity.x != 0 && OnGround());
    }
    #endregion

    #region Jump
    private void Jump()
    {
        UpdateIsJumpingState();
        SetVerticalVelocity();
        SetJumpAnimation();
    }

    private void UpdateIsJumpingState()
    {
        if (OnGround())
        {
            characterState.SetStateIsJumping(false);
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            airJumpCounter++;
            jumpBufferCounter = jumpBufferFrame;
        }
        else
            jumpBufferCounter -= Time.frameCount;
    }

    private void SetVerticalVelocity()
    {
        StartJump();
        EndJump();
    }

    private void StartJump()
    {
        if (!characterState.GetStateIsJumping()) // isJumping == true then jump.
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
                characterState.SetStateIsJumping(true);
            }
            else if (!OnGround() && airJumpCounter < maxAirJump && Input.GetKeyDown(KeyCode.Space))
            {
                characterState.SetStateIsJumping(true);
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            }
    }

    private void EndJump()
    {
        if (!Input.GetKeyUp(KeyCode.Space) || rigidbody.velocity.y <= 0) return;

        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        characterState.SetStateIsJumping(false);
    }

    private void SetJumpAnimation() { animator.SetBool("Jumping", !OnGround()); }
    #endregion

    #region Flip
    private void Flip()
    {
        if (axisHorizontal < 0)
            transform.localScale = new Vector2(-1, transform.localScale.y);
        else if (axisHorizontal > 0)
            transform.localScale = new Vector2(1, transform.localScale.y);
    }
    #endregion

    #region Dash
    private void StartDash()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashed)
        {
            StartCoroutine(Dash());
            isDashed = true;
        }

        if(OnGround()) isDashed = false;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        characterState.SetStateIsDashing(true);
        animator.SetTrigger("Dashing");
        rigidbody.gravityScale = 0;
        rigidbody.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        if (OnGround()) Instantiate(prefabDashEffect, transform);
        yield return new WaitForSeconds(dashTime);
        rigidbody.gravityScale = gravity;
        characterState.SetStateIsDashing(false);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    #endregion

    private bool OnGround()
    {
        if (Physics2D.Raycast(groundCheckEntity.position, Vector2.down, groundCheckYRayDistance, groundLayer)
            || Physics2D.Raycast(groundCheckEntity.position + new Vector3( groundCheckXRayDistance, 0, 0), Vector2.down, groundCheckYRayDistance, groundLayer) 
            || Physics2D.Raycast(groundCheckEntity.position + new Vector3(-groundCheckXRayDistance, 0, 0), Vector2.down, groundCheckYRayDistance, groundLayer))
            return true;
        else
            return false;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(groundCheckEntity.position, new Vector3(groundCheckEntity.position.x, groundCheckEntity.position.y - groundCheckYRayDistance, 0));
        Gizmos.DrawLine(groundCheckEntity.position, new Vector3(groundCheckEntity.position.x + groundCheckXRayDistance, groundCheckEntity.position.y, 0));
        Gizmos.DrawLine(groundCheckEntity.position, new Vector3(groundCheckEntity.position.x - groundCheckXRayDistance, groundCheckEntity.position.y, 0));
    }
#endif
}