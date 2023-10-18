using UnityEngine;


public abstract class BasePowerUp : MonoBehaviour, IPowerUp
{
    public PowerUpType type;
    public bool spawned = false;
    protected bool consumed = false;
    protected int moveRight = -1;
    protected Rigidbody2D powerUpRigidBody;
    protected Collider2D powerUpCollider;
    public Animator animator;

    // base methods
    protected virtual void Start()
    {
        powerUpRigidBody = GetComponent<Rigidbody2D>();
        powerUpCollider = GetComponent<Collider2D>();
    }

    // interface methods
    // 1. concrete methods
    public PowerUpType powerUpType
    {
        get // getter
        {
            return type;
        }
    }

    public bool hasSpawned
    {
        get // getter
        {
            return spawned;
        }
    }

    public void DestroyPowerup()
    {
        Destroy(this.gameObject);
    }

    // 2. abstract methods, must be implemented by derived classes
    public abstract void SpawnPowerup();
    public abstract void ApplyPowerup(MonoBehaviour i);

    public virtual void LevelRestart()
    {
        if (spawned)
        {
            spawned = false;
            consumed = false;
            moveRight = -1;
            animator.SetTrigger("reset");
        }
    }
}