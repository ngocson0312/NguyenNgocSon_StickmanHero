using SuperFight;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BossRedWitch : Boss
{
    public GameObject GroundFake;
    public GameObject GroundFake2;
    public GameObject GroundFake3;
    public LaserBeamNew laser;
    public FireBall acidProjectile;
    public Transform[] laserPosHigh;
    public Transform[] laserPosLow;
    public Transform centerPos;
    public ParticleSystem teleFx;
    public ParticleSystem fxDeath;
    private List<Transform> laserPosTemp = new List<Transform>();

    IEnumerator AttackAcction()
    {
        yield return new WaitForSeconds(1.5f);
        Action1();
        yield return new WaitForSeconds(4.2f);
        Action1();
        yield return new WaitForSeconds(4.2f);
        Action2();
        yield return new WaitForSeconds(7.5f);
        StartCoroutine(AttackAcction());
    }


    void Action1()
    {
        animatorHandle.PlayAnimation("Witch_Skill1", 0.1f, 1, true);
    }

    void Action2()
    {
        TeleportCenter();
        animatorHandle.PlayAnimation("Witch_Skill2", 0.1f, 1, true);
    }
    public override void Resume()
    {
        base.Resume();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
    public void Teleport()
    {
        CameraController.Instance.ShakeCamera(.35f, 0.7f, 10, 90, true);
        teleFx.Play();
        int randomNum = Random.Range(0, 2);
        if (PlayerManager.Instance.transform.position.y >= 0.5f)
        {
            transform.position = laserPosHigh[randomNum].position;
        }
        else
        {
            transform.position = laserPosLow[randomNum].position;
        }

        CheckRotate();
    }

    public void TeleportCenter()
    {
        teleFx.Play();
        transform.position = centerPos.position;
        CheckRotate();
    }

    public void SpawnFireball()
    {
        CameraController.Instance.ShakeCamera(3f, 0.35f, 10, 90, true);
        for (int i = 0; i < 12; i++)
        {
            float playerPos = PlayerManager.Instance.transform.position.x;
            if (playerPos <= transform.position.x)
            {
                StartCoroutine(_DelaySpawnFireball(-Vector3.right, 12f));
            }
            else
            {
                StartCoroutine(_DelaySpawnFireball(Vector3.right, -12f));
            }
        }
    }

    IEnumerator _DelaySpawnFireball(Vector3 dir, float pos)
    {
        yield return new WaitForSeconds(Random.Range(0, 1.5f));
        FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(acidProjectile.transform).GetComponent<FireBall>();
        fb.transform.position = transform.position + new Vector3(pos, Random.Range(-0.6f, 4f), 0);
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = stats.damage / 2;
        damageInfo.characterType = characterType;
        damageInfo.idSender = core.combat.getColliderInstanceID;
        fb.Initialize(dir, damageInfo);
    }

    public void ActiveLaser(bool active)
    {
        if (active) laser.Active(stats.damage / 2);
        else laser.Deactive();
    }

    public override void OnTakeDamage(DamageInfo damageInfo)
    {
        if (!isActive) return;
        base.OnTakeDamage(damageInfo);
        if (stats.currHealth <= 0)
        {
            if (GroundFake != null)
            {
                GroundFake.SetActive(false);
            }
            if (GroundFake3 != null)
            {
                GroundFake3.SetActive(true);
            }
            isActive = false;
            fxDeath.Play();
            GameplayCtrl.Instance.CreateCoinBoss(transform.position + new Vector3(0, 0, -1));
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effBossDie);
            animatorHandle.PlayAnimation("Die", 0.1f, 0, true);
            ActiveLaser(false);
            Die(true);
        }
        else
        {
            SoundManager.Instance.playRandFx(TYPE_RAND_FX.FX_TAKE_DAMAGE);
            animatorHandle.PlayAnimation("Hit", 0.1f, 0, true);
        }
    }

    void CheckRotate()
    {
        float playerPos = PlayerManager.Instance.transform.position.x;
        if (playerPos <= transform.position.x)
        {
            transform.GetChild(0).localRotation = Quaternion.Euler(0, 220, 0);
            laser.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.GetChild(0).localRotation = Quaternion.Euler(0, 140, 0);
            laser.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public override void Active()
    {
        isActive = true;
        if (GroundFake2 != null)
        {
            GroundFake2.SetActive(true);
        }
        StartCoroutine(AttackAcction());
    }

    protected override void UpdateLogic()
    {
    }

    protected override void UpdatePhysic()
    {
    }
}
