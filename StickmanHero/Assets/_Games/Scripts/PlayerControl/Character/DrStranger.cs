using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.Examples;

namespace SuperFight
{
    public class DrStranger : Character
    {
        public Transform shootPosition;
        public ParticleSystem muzzle;
        public FireBall fireBallKaisa;
        public FireBall drStrange_Skill;
        public Transform TargetSkill3;
        public Transform StartSkill3;
        public float distance = 2f;
        public SkeletonGhost skeletonGhost;
        public override void Initialize(Controller controller)
        {
            base.Initialize(controller);
        }
        private void Update()
        {
            skeletonGhost.enabled = true;
        }
        public void Shoot()
        {
            //muzzle.Play();
            FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBallKaisa.transform).GetComponent<FireBall>();
            fb.transform.position = shootPosition.position;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.characterType = controller.characterType;
            int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
            damageInfo.damage = controller.stats.damage + damageBonus;
            damageInfo.idSender = controller.core.combat.getColliderInstanceID;
            if (PlayerManager.Instance.transform.localScale.x == 1)
            {
                fb.Initialize(Vector2.right, damageInfo);
            }
            else
            {
                fb.Initialize(-Vector2.right, damageInfo);
            }
        }

        public void Missle()
        {
            //muzzle.Play();
            SpawnMissLe(Vector2.up * 0.1f);
            SpawnMissLe(Vector2.up * 0.2f);
            SpawnMissLe(Vector2.zero);
        }

        public void SpawnMissLe(Vector2 dir)
        {
            FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBallKaisa.transform).GetComponent<FireBall>();
            fb.transform.position = shootPosition.position;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.idSender = controller.core.combat.getColliderInstanceID;
            int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
            damageInfo.damage = controller.stats.damage + damageBonus;
            damageInfo.characterType = controller.characterType;
            if (PlayerManager.Instance.transform.localScale.x == 1)
            {
                fb.Initialize(Vector2.right + dir, damageInfo);
            }
            else
            {
                fb.Initialize(-Vector2.right + dir, damageInfo);
            }
        }
        void SendDamage()
        {
            var direc = (Vector2.right * PlayerManager.Instance.transform.localScale.x).normalized;
            for (int i = 0; i < 3; i++)
            {
                Vector3 _startPos = StartSkill3.position + Vector3.right * (distance * i * direc.x);
                Vector3 _targetPos = TargetSkill3.position + Vector3.right * (distance * i * direc.x);
                StartCoroutine(IE_SpawnMagic(Direction(_targetPos, _startPos), _startPos, i * .15f));
            }
        }
        Vector3 Direction(Vector3 _target, Vector3 _start)
        {
            return (_target - _start).normalized;
        }
        IEnumerator IE_SpawnMagic(Vector3 _dir, Vector3 _startPos, float _delay)
        {
            yield return new WaitForSeconds(_delay);
            SpawnMagic(_dir, _startPos);
        }
        void SpawnMagic(Vector2 dir, Vector3 _startPos)
        {
            FireBall fb = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(drStrange_Skill.transform).GetComponent<FireBall>();
            fb.transform.position = _startPos;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.idSender = controller.core.combat.getColliderInstanceID;
            int damageBonus = controller.stats.damage * currentWeapon.damage / 100;
            damageInfo.damage = controller.stats.damage + damageBonus;
            damageInfo.characterType = controller.characterType;
            fb.transform.rotation = Quaternion.LookRotation(dir);
            fb.Initialize(dir, damageInfo);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(StartSkill3.position, TargetSkill3.position);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(StartSkill3.position + Vector3.right * distance, TargetSkill3.position + Vector3.right * distance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(StartSkill3.position + Vector3.right * (distance*2), TargetSkill3.position + Vector3.right * (distance * 2));
        }
    }
}

