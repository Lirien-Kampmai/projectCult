using UnityEngine;

public class PlayerViewController : MonoBehaviour
{
    public Animator animator;
    public PlayerInfoModel playerInfoModel;


    private void Update()
    {
        Flip();
        SetAnimation();
    }

    private void SetAnimation()
    {
        switch (playerInfoModel.playerState)
        {
            case PlayerInfoModel.PlayerState.Walking:
                SetWalkAnimation();
                break;

            case PlayerInfoModel.PlayerState.Jumping:
                SetJumpAnimation();
                break;

            case PlayerInfoModel.PlayerState.Attacking:
                SetAttackAnimation();
                break;

            case PlayerInfoModel.PlayerState.Dashing:
                SetDashAnimation();
                break;
        }
    }

    private void Flip()
    {
        switch (playerInfoModel.lookingTo)
        {
            case PlayerInfoModel.PlayerLookingTo.LookingRight:
                transform.localScale = new Vector2(1, transform.localScale.y);
                break;

            case PlayerInfoModel.PlayerLookingTo.LookingLeft:
                transform.localScale = new Vector2(-1, transform.localScale.y);
                break;
        }
    }

    public void AttackEffectAtAngle(GameObject attackEffect, int effectAngle, Transform attackTransform)
    {
        attackEffect = Instantiate(attackEffect, attackTransform);
        attackEffect.transform.eulerAngles = new Vector3(0, 0, effectAngle);
        attackEffect.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    public void AttackEffect(GameObject attackEffect, Transform attackTransform)
    {
        Instantiate(attackEffect, attackTransform);
    }

    private void SetWalkAnimation() { animator.SetBool("Walking", playerInfoModel.rigidbody.velocity.x != 0 && playerInfoModel.OnGround); }

    private void SetJumpAnimation() { animator.SetBool("Jumping", !playerInfoModel.OnGround); }

    private void SetAttackAnimation() { animator.SetTrigger("Attacking"); }

    private void SetDashAnimation() { animator.SetTrigger("Dashing"); }
}