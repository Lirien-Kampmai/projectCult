using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactEffect : MonoBehaviour
{
    public PlayerInfoModel playerInfoModel;
    public PlayerInputController playerInputController;

    public bool isImpactRecoilingX;
    public bool isImpactRecoilingY;

    [Header("ImpactAttack")]
    [SerializeField] public int impactXSteps = 5;
    [SerializeField] public int impactYSteps = 5;
    [SerializeField] public float impactXSpeed = 100;
    [SerializeField] public float impactYSpeed = 100;
    int stepsXRecoiled, stepsYRecoiled;

    private void Impact()
    {
        if (isImpactRecoilingX)
        {
            switch (playerInfoModel.lookingTo)
            {
                case PlayerInfoModel.PlayerLookingTo.LookingRight:
                    playerInfoModel.GetComponent<Rigidbody>().velocity = new Vector2(-impactXSpeed, 0);
                    break;

                case PlayerInfoModel.PlayerLookingTo.LookingLeft:
                    playerInfoModel.GetComponent<Rigidbody>().velocity = new Vector2(impactXSpeed, 0);
                    break;
            }
        }

        if (isImpactRecoilingY)
        {
            playerInfoModel.rigidbody.gravityScale = 0;
            if (playerInputController.AxisY < 0)
            {
                playerInfoModel.GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x, impactYSpeed);
            }
            else
            {
                playerInfoModel.GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x, -impactYSpeed);
            }
        }
        else
        {
            playerInfoModel.rigidbody.gravityScale = playerInfoModel.gravity;
        }

        if (isImpactRecoilingX && stepsXRecoiled < impactXSteps)
            stepsXRecoiled++;
        else
            StopRecoilX();

        if (isImpactRecoilingY && stepsYRecoiled < impactYSteps)
            stepsYRecoiled++;
        else
            StopRecoilY();

        if (playerInfoModel.OnGround)
            StopRecoilY();
    }

    private void StopRecoilX()
    {
        stepsXRecoiled = 0;
        isImpactRecoilingX = false;
    }

    private void StopRecoilY()
    {
        stepsYRecoiled = 0;
        isImpactRecoilingY = false;
    }
}
