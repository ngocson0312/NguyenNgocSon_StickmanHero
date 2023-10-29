using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamNew : MonoBehaviour
{
    public LineRenderer warning, main;
    public ParticleSystem beamFx;

    public float warningTime;

    private void Awake()
    {
        main.gameObject.SetActive(false);
        warning.gameObject.SetActive(false);
        main.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void Active(int dmg)
    {
        main.GetComponent<Laser>().SetDamage(dmg);
        main.gameObject.SetActive(false);
        main.GetComponent<BoxCollider2D>().enabled = true;
        StartCoroutine(_Active());
    }

    public void Deactive()
    {
        main.GetComponent<BoxCollider2D>().enabled = false;
        main.GetComponent<Animation>().Play("Laser_Hide");
        beamFx.Stop();
    }

    IEnumerator _Active()
    {
        warning.gameObject.SetActive(true);
        yield return new WaitForSeconds(warningTime);
        beamFx.Play();
        warning.gameObject.SetActive(false);
        main.gameObject.SetActive(true);
        main.GetComponent<Animation>().Play("Laser_Show");
    }
}
