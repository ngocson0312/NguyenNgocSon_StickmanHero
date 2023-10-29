using PathologicalGames;
using UnityEngine;
using System.Collections;
public abstract class Projectile : MonoBehaviour
{
    public float speed;
    public GameObject display;
    public GameObject impact;
    public bool isActive;
    protected bool isPooled;
    protected DamageInfo damageInfo;
    protected Vector2 direction;
    public LayerMask layerContact;
    float lifeTimer;
    public virtual void Initialize(Vector2 direction, DamageInfo damageInfo)
    {
        this.damageInfo = damageInfo;
        this.direction = direction;
        GetComponent<Collider2D>().enabled = true;
        if (display != null)
            display.SetActive(true);
        if (impact != null)
            impact.SetActive(false);
        isActive = true;
        isPooled = false;
        lifeTimer = 0;
    }
    public virtual void UpdateLogic()
    {
        if (isActive)
        {
            transform.position += speed * Time.deltaTime * (Vector3)direction;
        }
    }
    private void Update()
    {
        UpdateLogic();
        HandleLifeTime();
    }
    protected abstract void OnCollision(Collider2D other);
    private void OnTriggerEnter2D(Collider2D other)
    {
        OnCollision(other);
    }
    protected void Deactive(float time)
    {
        lifeTimer = time;
        if (time <= 0)
        {
            isPooled = true;
            PoolManager.Pools["Projectile"].Despawn(transform, PoolManager.Pools["Projectile"].transform);
        }
    }
    void HandleLifeTime()
    {
        if (lifeTimer > 0 && !isPooled)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0)
            {
                lifeTimer = 0;
                isPooled = true;
                PoolManager.Pools["Projectile"].Despawn(transform, PoolManager.Pools["Projectile"].transform);
            }
        }
    }
}
