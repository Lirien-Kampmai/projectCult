using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfoModel : Entity
{
    private float impactRecoilTimer;
    [SerializeField] private Transform checkEntity;

    public bool isPlayer = false;

    [SerializeField] private float checkXRayDistance = 0.2f;

    [SerializeField] private LayerMask layerPayer;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        ImpactRecoiling();
        UpdateLookingTo();
        CheckPlayer();
    }

    private void ImpactRecoiling()
    {
        if (!isImpactRecoiling) return;

        if (impactRecoilTimer < impactRecoilLenght)
        {
            impactRecoilTimer += Time.deltaTime;
        }
        else
        {
            isImpactRecoiling = false;
            impactRecoilTimer = 0;
        }
    }

    public void Move(Vector2 dir)
    {
        rigidbody.velocity = new Vector2(speed / dir.x, rigidbody.velocity.y);

        if (dir == null)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }
    }

    private void CheckPlayer()
    {
        if (Physics2D.Raycast(checkEntity.position, rigidbody.velocity, checkXRayDistance, layerPayer))
            isPlayer = true;
        else
            isPlayer = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GizmosRayToGround();
    }
    private void GizmosRayToGround()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(checkEntity.position, new Vector3(checkEntity.position.x + checkXRayDistance, checkEntity.position.y, 0));
    }
#endif
}