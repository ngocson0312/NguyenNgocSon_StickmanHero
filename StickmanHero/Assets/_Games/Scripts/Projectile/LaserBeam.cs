using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public LayerMask damageLayer;
    public LineRenderer beam;
    public ParticleSystem beamStart;
    public ParticleSystem beamEnd;
    public float activeTime = 1f;
    private float timer;
    public int state;
    public float angle;
    float currentAngle;
    int facingDirection;
    DamageInfo damageInfo;
    List<Collider2D> listColl;
    public void Active(int direction, DamageInfo damageInfo)
    {
        gameObject.SetActive(true);
        beam.SetPosition(0, transform.position);
        beam.SetPosition(1, transform.position);
        beamStart.Play();
        beamEnd.Stop();
        this.damageInfo = damageInfo;
        facingDirection = direction;
        state = 1;
        timer = -0.5f;
        currentAngle = angle;
    }
    private void Update()
    {
        if (state == 1)
        {
            timer += Time.deltaTime;
            listColl = new List<Collider2D>();
            if (timer >= 0)
            {
                timer = activeTime;
                state = 2;
                //beamStart.Stop();
                beamStart.transform.position = new Vector3(beamStart.transform.position.x, beamStart.transform.position.y, -0.5f);
                beamEnd.Play();
            }
        }

        if (state == 2)
        {
            beam.SetPosition(0, transform.position);
            beam.SetPosition(1, transform.position);
            currentAngle -= Time.deltaTime * 5;
            Vector2 direction = new Vector2(Mathf.Sin(currentAngle * Mathf.Deg2Rad) * facingDirection, Mathf.Cos(currentAngle * Mathf.Deg2Rad));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 10f, damageLayer);
            if (hit.transform != null)
            {
                beam.SetPosition(1, hit.point);
                beamEnd.transform.position = hit.point;
                if (hit.transform.GetComponent<IDamage>() != null && !listColl.Contains(hit.collider))
                {
                    damageInfo.stunTime = 1;
                    damageInfo.hitDirection = facingDirection;
                    hit.transform.GetComponent<IDamage>().TakeDamage(damageInfo);
                    listColl.Add(hit.collider);
                }
            }
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = 0;
                    Deactive();
                }
            }
        }

    }
    private void OnDrawGizmos()
    {
        Vector2 direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        Gizmos.DrawRay(transform.position, direction * 10);
    }
    public void Deactive()
    {
        state = 0;
        gameObject.SetActive(false);
    }
}
