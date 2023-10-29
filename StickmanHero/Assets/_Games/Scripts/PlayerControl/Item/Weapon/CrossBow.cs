// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// namespace SuperFight
// {
//     [CreateAssetMenu(fileName = "Weapon", menuName = "Equipment/CrossBow", order = 0)]
//     public class CrossBow : Weapon
//     {
//         public Arrow arrowPrefab;
//         public Vector3 offsetPos;
//         public bool isStraigh = false;
//         // public override void TriggerSkill(PlayerManager user)
//         // {
//         //     // Arrow arrow = PoolingObject.GetObjectFree(arrowPrefab);
//         //     // // arrow.transform.position = user.transform.position + new Vector3(offsetPos.x * user.playerMovement.direction, offsetPos.y);
//         //     // Collider2D[] colls = new Collider2D[1];
//         //     // Physics2D.OverlapCircleNonAlloc(user.transform.position, attackRange, colls, user.targetLayer);
//         //     // DamageInfo damageInfo = new DamageInfo();
//         //     // damageInfo.damage = damage + user.playerStats.stats.damage;
//         //     // // Vector3 direction = new Vector3(user.playerMovement.direction, 0, 0);
//         //     // if (colls[0] != null)
//         //     // {
//         //     //     var dir = colls[0].transform.position - user.transform.position;
//         //     //     if (Vector2.Dot(Vector2.right * user.playerMovement.direction, (dir)) > 0)
//         //     //     {
//         //     //         direction = dir.normalized;
//         //     //     }
//         //     // }
//         //     // if (!isStraigh)
//         //     // {
//         //     //     arrow.Active(user.gameObject.GetInstanceID(), direction, damageInfo);
//         //     // }
//         //     // else
//         //     // { 
//         //     //     arrow.Active(user.gameObject.GetInstanceID(), user.playerMovement.direction * Vector3.right, damageInfo);
//         //     // }
//         // }
//     }
// }

