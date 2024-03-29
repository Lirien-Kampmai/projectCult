using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactEffect : MonoBehaviour
{
    public Entity playerInfoModel;
    public PlayerInputController playerInputController;

    public bool isImpactRecoilingX;
    public bool isImpactRecoilingY;

    [Header("ImpactAttack")]
    [SerializeField] public int impactXSteps = 5;
    [SerializeField] public int impactYSteps = 5;
    [SerializeField] public float impactXSpeed = 100;
    [SerializeField] public float impactYSpeed = 100;
    int stepsXRecoiled, stepsYRecoiled;

    private void Update()
    {
        Impact();
    }

    private void Impact()
    {
        if (isImpactRecoilingX)
        {
            switch (playerInfoModel.lookingTo)
            {
                case Entity.PlayerLookingTo.LookingRight:
                    playerInfoModel.SetRigidbodyVelocity(new Vector2(-impactXSpeed, 0));
                    break;

                case Entity.PlayerLookingTo.LookingLeft:
                    playerInfoModel.SetRigidbodyVelocity(new Vector2(impactXSpeed, 0));
                    break;
            }
        }

        if (isImpactRecoilingY)
        {
            if (playerInputController.AxisY < 0)
                playerInfoModel.SetRigidbodyVelocity(new Vector2(playerInfoModel.GetRigitbodyVelocity().x, impactYSpeed));
            else
                playerInfoModel.SetRigidbodyVelocity(new Vector2(playerInfoModel.GetRigitbodyVelocity().x, -impactYSpeed));
        }

        CheckStopRecoil();
    }

    private void CheckStopRecoil()
    {
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
