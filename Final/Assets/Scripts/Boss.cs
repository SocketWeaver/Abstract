using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IHealth
{
    public enum BossState { Idle, Attack, Rest, TransitToRest, TransitToAttack };

    [Header("AI")]
    public float TransitSpeed = 10f;
    public Vector2 AttackPosition;
    public Vector2 RestPosition;
    public float AttackInterval = 1f;
    public float AttackTime = 5f;
    public float RestTime = 5f;
    public float TargetPositionSnapDistance = 0.01f;
    public float stateTimer = 0;
    public BossState bossState;
    Vector2 targetPosition;

    [Header("Health")]
    public int MaxHealth = 30;
    HealthBar healthBar;
    int currentHealth = 0;

    [Header("Attack")]
    public GameObject Bullet;
    public Transform FirePosition;
    public float BulletSpeed = 10.0f;
    [Range(2, 15)]
    public int NumberOfBullets = 5;
    [Range(10, 80)]
    public float AttackAngle = 60;

    [Header("Hit")]
    public Material HitMaterial;
    public float HitEffectDisplayTime = 0.2f;
    public GameObject Explosion;
    float hitEffectTimer = 0;
    SpriteRenderer spriteRenderer;
    Material defaultMaterial;

    void Start()
    {
        bossState = BossState.Idle;
        healthBar = GetComponentInChildren<HealthBar>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        currentHealth = MaxHealth;
        UpdateHealthBar(currentHealth);
    }

    void Update()
    {
        StopHitEffectIfNecessary();
        BossAI();
    }

    public void OnPlayerEntered(Player player)
    {
        Debug.Log("Player entered");
        bossState = BossState.TransitToAttack;
        targetPosition = AttackPosition;
    }

    void BossAI()
    {
        switch (bossState)
        {
            case BossState.Attack:
                {
                    DoAttack();
                    break;
                }
            case BossState.Rest:
                {
                    DoRest();
                    break;
                }
            case BossState.TransitToAttack:
                {
                    MoveToTargetPosition();
                    if (SnapToTargetPosition())
                    {
                        stateTimer = 0;
                        bossState = BossState.Attack;
                    }
                    break;
                }
            case BossState.TransitToRest:
                {
                    MoveToTargetPosition();
                    if (SnapToTargetPosition())
                    {
                        stateTimer = 0;
                        bossState = BossState.Rest;
                    }
                    break;
                }
            case BossState.Idle:
                {
                    break;
                }
        }
    }

    void DoAttack()
    {
        stateTimer += Time.deltaTime;
        if (stateTimer > AttackTime)
        {
            bossState = BossState.TransitToRest;
            targetPosition = RestPosition;
            CancelInvoke("Fire");
            return;
        }

        if (!IsInvoking("Fire"))
        {
            InvokeRepeating("Fire", 0f, AttackInterval);
        }
    }

    void DoRest()
    {
        stateTimer += Time.deltaTime;
        if (stateTimer > RestTime)
        {
            bossState = BossState.TransitToAttack;
            targetPosition = AttackPosition;
            return;
        }
    }

    void MoveToTargetPosition()
    {
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * TransitSpeed);
    }

    bool SnapToTargetPosition()
    {
        float distance = Vector2.Distance(transform.position, targetPosition);

        if (distance < TargetPositionSnapDistance)
        {
            transform.position = targetPosition;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Fire()
    {
        for(int index = 0; index < NumberOfBullets; index++)
        {
            GameObject bullet = Instantiate(Bullet, FirePosition.position, FirePosition.rotation);
            Rigidbody2D rb2D = bullet.GetComponent<Rigidbody2D>();
            float degree = 270 - AttackAngle;
            degree += 2 * AttackAngle / (NumberOfBullets-1) * index;
            Vector2 direction = DegreeToDirection(degree);
            rb2D.velocity = direction * BulletSpeed;
        }
    }

    Vector2 DegreeToDirection(float degree)
    {
        return new Vector2(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad));
    }

    public void TakeDamage(int damage)
    {
        if(bossState == BossState.Idle)
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, MaxHealth);
        UpdateHealthBar(currentHealth);

        StartHitEffect();

        if (currentHealth == 0)
        {
            Die();
        }
    }

    void StopHitEffectIfNecessary()
    {
        hitEffectTimer += Time.deltaTime;

        if (hitEffectTimer > HitEffectDisplayTime)
        {
            StopHitEffect();
        }
    }

    void StopHitEffect()
    {
        spriteRenderer.material = defaultMaterial;
    }

    void StartHitEffect()
    {
        hitEffectTimer = 0;
        spriteRenderer.material = HitMaterial;
    }

    void Die()
    {
        if (Explosion != null)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void UpdateHealthBar(int health)
    {
        if (healthBar != null)
        {
            float percentage = (float)health / (float)MaxHealth;
            healthBar.UpdateHealth(percentage);
        }
    }
}
