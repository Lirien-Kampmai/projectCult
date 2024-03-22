using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseJump : MonoBehaviour
{



    [Header("Jump")]
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float canisTime = 0.3f;
    [SerializeField] private float canJumpTime;
    [SerializeField] private int maxSecondJumps = 1;
    private int SecondJumpsBuffer;
    private float jumpBufferTime;
    private float bufferCanisTime;
    private bool isJumping;
    private bool isCanis = true;
    private bool canFirstJump;
    private bool canSecondJump;
    private bool isLockSecondJump = false;
    private PlayerInfoModel playerInfoModel;
    private PlayerInputController inputController;

    private void Start()
    {
        playerInfoModel = GetComponent<PlayerInfoModel>();
        inputController = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
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
        if(inputController.IsDownSpace kl canFirstJump)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }


        if (!isJumping && canFirstJump && isCanis)
        {
            playerInfoModel.rigidbody.velocity = new Vector2(playerInfoModel.rigidbody.velocity.x, jumpForce);
        }

        if (inputController.IsUpSpace && playerInfoModel.rigidbody.velocity.y > 0)
        {
            playerInfoModel.rigidbody.velocity = new Vector2(playerInfoModel.rigidbody.velocity.x, 0);
        }
    }

    public void SecondJump()
    {
        if(inputController.IsUpSpace && canSecondJump && isJumping)
        {
            playerInfoModel.rigidbody.velocity = new Vector2(playerInfoModel.rigidbody.velocity.x, jumpForce);
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
            if (!playerInfoModel.OnGround && SecondJumpsBuffer < maxSecondJumps)
                canSecondJump = true;
            else
                canSecondJump = false;
        }
    }

    private void CalculateJumpFrames()
    {
        if (inputController.IsDownSpace)
            jumpBufferTime = canJumpTime;
    }
    private void CanisState()
    {
        CalculateCanisTime();

        if (bufferCanisTime >= 0)
            isCanis = true;
        else
            isCanis = false;
    }
    private void CalculateCanisTime()
    {
        if (playerInfoModel.OnGround)
        {
            SecondJumpsBuffer = 0;
            bufferCanisTime = canisTime;
        }
        else
            bufferCanisTime -= Time.deltaTime;
    }
    public void UnlockSecondJump() { isLockSecondJump = true;  }
    public void LockSecondJump()   { isLockSecondJump = false; }
}
