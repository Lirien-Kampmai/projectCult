using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyInfoModel))]
public class EnemyAILogic : MonoBehaviour
{
    // type AI behaviour
    public enum AIBehaviour
    {
        Null,
        Attack
    }

    public AIBehaviour aIBehaviour;

    // длинна рэйкаста
    public float evadeRayLength;

    public Rigidbody2D rigitbodytarget;

    public EnemyInfoModel enemyIM;

    public Vector2 movePosition;

    public Entity selectedTarget;
    public AttackLogic attackLogic;


    private void Start()
    {
        enemyIM = GetComponent<EnemyInfoModel>();
        attackLogic = GetComponent<AttackLogic>();
        enemyIM.transform.SetParent(null);
        aIBehaviour = AIBehaviour.Null;
    }

    private void Update()
    {
        UpdateAi();
    }

    private void UpdateAi()
    {
        if (aIBehaviour == AIBehaviour.Null) aIBehaviour = AIBehaviour.Attack;
        if (aIBehaviour == AIBehaviour.Attack) UpdateAIAttack();
    }

    public void UpdateAIAttack()
    {
        SelectTarget();


        ActionFindMovePosition();

        ActionFire();
    }

    private void SelectTarget()
    {
        selectedTarget = FindNearestDestructibleTarget();
    }
    private Entity FindNearestDestructibleTarget()
    {
        Entity potentialTarget = null;

        foreach (var find in Entity.AllEntity)
        {
            if (find.GetComponent<EnemyInfoModel>() == enemyIM) continue;

            potentialTarget = find;
            rigitbodytarget = find.GetComponent<Rigidbody2D>();
        }
        return potentialTarget;
    }

    // метод стрельбы ИИ
    private void ActionFire()
    {
        if (selectedTarget == null) return;

        if (enemyIM.isPlayer)
            attackLogic.Attack();

    }


    private void ActionFindMovePosition()
    {
        if (aIBehaviour == AIBehaviour.Attack)
        {
            if (selectedTarget != null)
                enemyIM.Move(selectedTarget.transform.position);
        }
    }

    // дефолтный уклонятор вправо от препятствия
    private void ActionEvadeCollision()
    {
        if (Physics2D.Raycast(transform.position, transform.forward, evadeRayLength) == true)
            movePosition = transform.position + (transform.right * 100.0f);
    }
}
