using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class UltimateRainbow : Projectile
{
    public float duration = 1f;
    public float delay = .5f;
    public GameObject Line;
    public override void Initialize(Vector2 direction, DamageInfo damageInfo)
    {
        base.Initialize(direction, damageInfo);
        GetComponent<Collider2D>().enabled = false;
        Line.SetActive(false);
        Invoke(nameof(SetLine), delay);
        Deactive(duration);
    }
    void SetLine()
    {
        GetComponent<Collider2D>().enabled = true;
        Line.SetActive(true);
    }
    protected override void OnCollision(Collider2D other)
    {
        throw new System.NotImplementedException();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;
        if ((layerContact & (1 << other.gameObject.layer)) == 0) return;
        if (other.GetInstanceID().Equals(damageInfo.idSender)) return;
        //isActive = false;
        //display.SetActive(false);
        //impact.SetActive(true);
        if (!other.GetInstanceID().Equals(damageInfo.idSender))
        {
            other.GetComponent<IDamage>()?.TakeDamage(damageInfo);
        }
    }
}
