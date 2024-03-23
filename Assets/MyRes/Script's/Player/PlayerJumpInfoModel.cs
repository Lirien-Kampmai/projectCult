using UnityEngine;

[RequireComponent(typeof(PlayerInputController), typeof (PlayerInfoModel))]
public class PlayerJumpInfoModel : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpForce      = 30f;
    [SerializeField] private float canisTime      = 0.3f;
    [SerializeField] private float canJumpTime    = 1;
    [SerializeField] private int   maxSecondJumps = 1;

    private float bufferCanisTime;
    private float jumpBufferTime;

    private int firstJumpBuffer;
    private int SecondJumpsBuffer;

    private bool isJumping;
    private bool isDashing;
    private bool canFirstJump;
    private bool canSecondJump;
    private bool isCanis = true;
    private bool isLockSecondJump = false;

    private PlayerInfoModel       playerInfoModel;
    private PlayerInputController inputController;

    private void Start()
    {
        playerInfoModel = GetComponent<PlayerInfoModel>();
        inputController = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
        playerInfoModel.IsJumping = isJumping;
        playerInfoModel.IsDashing = isDashing;


        CanisState();
        CalculateJumpFrames();

        CanFirstJumpCheck();
        CanSecondJumpCheck();
        
        FirstJump();
        SecondJump();
    }

    private void FixedUpdate()
    {
        if(!playerInfoModel.OnGround)
            jumpBufferTime -= Time.fixedDeltaTime;
    }

    public void FirstJump()
    {
        if(!playerInfoModel.OnGround)
            isJumping = true;
        else isJumping = false;


        if (!isJumping && canFirstJump && isCanis && !isDashing && firstJumpBuffer <= 0)
        {
            playerInfoModel.SetRigidbodyVelocity(new Vector2(playerInfoModel.GetRigitbodyVelocity().x, jumpForce));
            firstJumpBuffer++;
        }
            
        if (inputController.IsUpSpace && playerInfoModel.GetRigitbodyVelocity().y > 0)
            playerInfoModel.SetRigidbodyVelocity(new Vector2(playerInfoModel.GetRigitbodyVelocity().x, 0));
    }

    public void SecondJump()
    {
        if(inputController.IsDownSpace && canSecondJump && isJumping && !isDashing)
        {
            playerInfoModel.SetRigidbodyVelocity(new Vector2(playerInfoModel.GetRigitbodyVelocity().x, jumpForce));
            SecondJumpsBuffer++;
        }
    }

    private void CanFirstJumpCheck()
    {
        if (playerInfoModel.OnGround && jumpBufferTime > 0)
            canFirstJump = true;
        else
            canFirstJump = false;
    }

    private void CanSecondJumpCheck()
    {
        if(!isLockSecondJump)
        {
            if (!playerInfoModel.OnGround && SecondJumpsBuffer < maxSecondJumps && isJumping)
                canSecondJump = true;
            else
                canSecondJump = false;
        }
    }

    private void CalculateJumpFrames()
    {
        if (inputController.IsDownSpace && playerInfoModel.OnGround)
            jumpBufferTime = canJumpTime;
    }
    private void CanisState()
    {
        CalculateCanisTime();

        if (bufferCanisTime >= 0 && !canSecondJump)
            isCanis = true;
        else
            isCanis = false;
    }
    private void CalculateCanisTime()
    {
        if (playerInfoModel.OnGround)
        {
            SecondJumpsBuffer = 0;
            firstJumpBuffer = 0;
            bufferCanisTime = canisTime;
        }
        else
            bufferCanisTime -= Time.deltaTime;
    }
    public void UnlockSecondJump() { isLockSecondJump = true;  }
    public void LockSecondJump()   { isLockSecondJump = false; }
}