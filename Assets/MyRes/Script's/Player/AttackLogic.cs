using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityViewController))]
public class AttackLogic : MonoBehaviour
{
    private Entity InfoModel;
    [SerializeField] PlayerImpactEffect impactEffect;

    [Header("Attack")]

    [SerializeField] private Transform sideAttackTransform;

    [SerializeField] private Vector2 sideAttackArea;
    [SerializeField] private LayerMask entityLayer;


    private EntityViewController viewController;

    private void Start()
    {
        InfoModel = GetComponent<Entity>();
        viewController = GetComponent<EntityViewController>();
    }


    public void Attack()
    {
        Hit(sideAttackTransform, sideAttackArea, ref impactEffect.isImpactRecoilingX, impactEffect.impactXSpeed);
    }

    private void Hit(Transform attackTransform, Vector2 attackArea, ref bool impactDir, float impactStrenght)
    {
        Collider2D[] objToHit = Physics2D.OverlapBoxAll(attackTransform.position, attackArea, 0, entityLayer);

        if (objToHit.Length > 0)
            impactDir = true;

        for (int i = 0; i < objToHit.Length; i++)
            if (objToHit[i].GetComponent<Entity>() != null)
                objToHit[i].GetComponent<Entity>().EntityHit(InfoModel.Damage, InfoModel.ImpactRecoilFactor, (transform.position - objToHit[i].transform.position).normalized, impactStrenght, InfoModel.GetRigitbody());
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() { GizmosAttackArea(); }
    private void GizmosAttackArea()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(sideAttackTransform.position, sideAttackArea);
    }
#endif
}