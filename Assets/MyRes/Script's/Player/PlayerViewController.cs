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

        if (playerInfoModel.IsWalking && !playerInfoModel.IsJumping && !playerInfoModel.IsDashing && !playerInfoModel.IsAttacking)
            SetWalkAnimation();

        if (!playerInfoModel.IsJumping && !playerInfoModel.IsWalking && !playerInfoModel.IsDashing && !playerInfoModel.IsAttacking)
            SetIdleAnimation();

        if (playerInfoModel.IsJumping)
            SetJumpAnimation();

        if(playerInfoModel.IsDashing)
            SetDashAnimation();

        if(playerInfoModel.IsAttacking)
            SetAttackAnimation();
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

    public void AttackEffect(GameObject attackEffect, Transform attackTransform) { Instantiate(attackEffect, attackTransform); }

    private void SetWalkAnimation() { animator.SetTrigger("Walking"); }
    private void SetIdleAnimation() { animator.SetTrigger("Idle"); }
    private void SetJumpAnimation() { animator.SetTrigger("Jumping"); }
    private void SetAttackAnimation() { animator.SetTrigger("Attacking"); }
    private void SetDashAnimation() { animator.SetTrigger("Dashing"); }
}