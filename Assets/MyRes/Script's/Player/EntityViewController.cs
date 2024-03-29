using UnityEngine;

public class EntityViewController : MonoBehaviour
{
    private Animator animator;
    private Entity infoModel;

    private void Start()
    {
        animator = GetComponent<Animator>();
        infoModel = GetComponent<Entity>();
    }

    private void Update()
    {
        Flip();
        SetAnimation();
    }

    private void SetAnimation()
    {

        if (infoModel.IsWalking && !infoModel.IsJumping && !infoModel.IsDashing && !infoModel.IsAttacking)
            SetWalkAnimation();

        if (!infoModel.IsJumping && !infoModel.IsWalking && !infoModel.IsDashing && !infoModel.IsAttacking)
            SetIdleAnimation();

        if (infoModel.IsJumping)
            SetJumpAnimation();

        if(infoModel.IsDashing)
            SetDashAnimation();

        if(infoModel.IsAttacking)
            SetAttackAnimation();
    }

    private void Flip()
    {
        switch (infoModel.lookingTo)
        {
            case Entity.PlayerLookingTo.LookingRight:
                transform.localScale = new Vector2(1, transform.localScale.y);
                break;

            case Entity.PlayerLookingTo.LookingLeft:
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