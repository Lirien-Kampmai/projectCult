using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    [SerializeField] private PlayerInputController inputController;
    [SerializeField] private PlayerViewController viewController;
    [SerializeField] private PlayerInfoModel playerInfoModel;
    [SerializeField] PlayerImpactEffect impactEffect;

    [Header("Attack")]
    [SerializeField] private GameObject prefabAttackEffect;
    [SerializeField] private Transform upAttackTransform;
    [SerializeField] private Transform downAttackTransform;
    [SerializeField] private Transform sideAttackTransform;
    [SerializeField] private Vector2 upAttackArea;
    [SerializeField] private Vector2 downAttackArea;
    [SerializeField] private Vector2 sideAttackArea;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float damage;
    [SerializeField] private int upAndDownAttackAngle;// recomended value ~90
    private float timeBetweenAttack;
    private float timeSinceAttack;

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (inputController.IsLeftMouseButtonDown && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0;

            if (inputController.AxisY == 0 || inputController.AxisY < 0 && playerInfoModel.OnGround)
            {
                Hit(sideAttackTransform, sideAttackArea, ref impactEffect.isImpactRecoilingX, impactEffect.impactXSpeed);
                viewController.AttackEffect(prefabAttackEffect, sideAttackTransform);
            }
            else if (inputController.AxisY > 0)
            {
                Hit(upAttackTransform, upAttackArea, ref impactEffect.isImpactRecoilingY, impactEffect.impactYSpeed);
                viewController.AttackEffectAtAngle(prefabAttackEffect, upAndDownAttackAngle, upAttackTransform);
            }
            else if (inputController.AxisY < 0 && !playerInfoModel.OnGround)
            {
                Hit(downAttackTransform, downAttackArea, ref impactEffect.isImpactRecoilingY, impactEffect.impactYSpeed);
                viewController.AttackEffectAtAngle(prefabAttackEffect, -upAndDownAttackAngle, downAttackTransform);
            }
        }
    }

    private void Hit(Transform attackTransform, Vector2 attackArea, ref bool impactDir, float impactStrenght)
    {
        Collider2D[] objToHit = Physics2D.OverlapBoxAll(attackTransform.position, attackArea, 0, enemyLayer);

        if (objToHit.Length > 0)
        {
            impactDir = true;
        }

        for (int i = 0; i < objToHit.Length; i++)
        {
            if (objToHit[i].GetComponent<EnemyTest>() != null)
            {
                objToHit[i].GetComponent<EnemyTest>().EnemyHit(damage, (transform.position - objToHit[i].transform.position).normalized, impactStrenght);
            }
        }
    }

    private void OnDrawGizmos()
    {
        GizmosAttackArea();
    }

    private void GizmosAttackArea()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(sideAttackTransform.position, sideAttackArea);
        Gizmos.DrawWireCube(upAttackTransform.position, upAttackArea);
        Gizmos.DrawWireCube(downAttackTransform.position, downAttackArea);
    }

}
