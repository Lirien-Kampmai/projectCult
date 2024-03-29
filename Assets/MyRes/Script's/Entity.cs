using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    public enum PlayerLookingTo
    {
        LookingLeft,
        LookingRight
    }

    [Header("Base stats")]
    [SerializeField] protected float startHealth;
                     protected float currentHealth;
    [SerializeField] protected float speed;

    [SerializeField] protected float damage;
    public float Damage => damage;

    [SerializeField] protected float impactRecoilLenght;
    [SerializeField] protected float impactRecoilFactor;
    public float ImpactRecoilFactor => impactRecoilFactor;

    [SerializeField] protected bool isImpactRecoiling = false;
    [SerializeField] protected bool isDestroyable = true;

    protected Rigidbody2D rigidbody;
    protected float gravity;

    public PlayerLookingTo lookingTo;

    public bool OnGround;
    public bool IsWalking;
    public bool IsJumping;
    public bool IsDashing;
    public bool IsAttacking;

    protected virtual void Awake() 
    {
        currentHealth = startHealth;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void ApplyDamage(float damage)
    {
        if (isDestroyable == false) return;
        
        currentHealth -= damage;
        if (currentHealth <= 0) Kill();
    }

    public void EntityHit(float damage, float impactRecoilFactor, Vector2 hitDirection, float hitForce, Rigidbody2D rigidbody)
    {
        rigidbody.AddForce(-hitForce * impactRecoilFactor * hitDirection);
        ApplyDamage(damage);
    }

    protected virtual void Kill() { Destroy(gameObject); }

    private static HashSet<Entity> allEntity;
    public static IReadOnlyCollection<Entity> AllEntity => allEntity;

    protected virtual void OnEnable()
    {
        if (allEntity == null)
            allEntity = new HashSet<Entity>();

        allEntity.Add(this);
    }

    protected virtual void OnDestroy() { allEntity.Remove(this); }

    public void SetGravityScale(float setGravity)
    {
        rigidbody.gravityScale = setGravity;
    }

    public void ResetGravityScale()
    {
        rigidbody.gravityScale = gravity;
    }

    public void SetRigidbodyVelocity(Vector2 vector2)
    {
        rigidbody.velocity = vector2;
    }

    public Vector2 GetRigitbodyVelocity()
    {
        return rigidbody.velocity;
    }

    public Rigidbody2D GetRigitbody()
    {
        return rigidbody;
    }

    public void UpdateLookingTo()
    {
        if (rigidbody.velocity.x > 0)
            lookingTo = PlayerLookingTo.LookingRight;

        if (rigidbody.velocity.x < 0)
            lookingTo = PlayerLookingTo.LookingLeft;
    }
}