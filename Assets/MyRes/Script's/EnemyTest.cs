using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] float healh;
    [SerializeField] float impactRecoilLenght;
    [SerializeField] float impactRecoilFactor;
    [SerializeField] bool isImpactRecoiling = false;

    private float impactRecoilTimer;
    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(healh <=0)
            Destroy(gameObject);

        if(isImpactRecoiling)
        {
            if(impactRecoilTimer < impactRecoilLenght)
            {
                impactRecoilTimer += Time.deltaTime;
            }
            else
            {
                isImpactRecoiling = false;
                impactRecoilTimer = 0;
            }
        }
    }

    public void EnemyHit(float damage, Vector2 hitDirection, float hitForce)
    {
        healh -= damage;

        if(!isImpactRecoiling)
        {
            rigidbody.AddForce(-hitForce * impactRecoilFactor * hitDirection);
        }







    }
}
