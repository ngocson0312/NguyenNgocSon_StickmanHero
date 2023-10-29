using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class BossFightArena : MonoBehaviour
    {
        public float arenaRangeX = 10f;
        public float arenaRangeY = 10f;
        public bool lockCamera;
        public Vector2 offsetCamera;
        public Transform center;
        public Boss[] bosses;
        private int bossCount;
        FillBar healthBar;
        float totalHealth = 0;
        float currentHealth = 0;
        private bool battleActive;
        private bool startBattle;
        public float timeDelayBattleActive;
        public void Initialize()
        {
            bossCount = bosses.Length;
            battleActive = false;
            startBattle = false;
            currentHealth = 0;
            totalHealth = 0;
            for (int i = 0; i < bosses.Length; i++)
            {
                bosses[i].Initialize(this);
                totalHealth += bosses[i].stats.health;
            }
            currentHealth = totalHealth;
            PlayScreenCtl ingameScreen = ScreenUIManager.Instance.GetScreen<PlayScreenCtl>(ScreenName.PLAYSCREEN);
            healthBar = ingameScreen.bossHealthBar;
        }
        private void Update()
        {
            if (PlayerManager.Instance == null || battleActive) return;
            Bounds bound = new Bounds(center.position, new Vector3(arenaRangeX, arenaRangeY, 0));
            Vector2 playerPos = PlayerManager.Instance.transform.position;
            if (bound.Contains(playerPos))
            {
                OnTriggerBattle();
            }
        }
        IEnumerator IEDelayBattleActive()
        {
            ScreenUIManager.Instance.ShowScreen(ScreenName.PLAYSCREEN);
            yield return new WaitForSeconds(timeDelayBattleActive);
            for (int i = 0; i < bosses.Length; i++)
            {
                bosses[i].Active();
            }
            battleActive = true;
        }
        public void OnTriggerBattle()
        {
            healthBar.InitializeBar(true);
            Bounds bound = new Bounds(center.position, new Vector3(arenaRangeX, arenaRangeY, 0));
            StartCoroutine(IEDelayBattleActive());
            if (lockCamera)
            {
                CameraController.Instance.SetTargetFollow(center);
            }
            CameraController.Instance.SetOffset(offsetCamera);
            CameraController.Instance.SetOrthoSize(bound.size, 1.5f, true);
        }
        public void OnPauseGame()
        {
            for (int i = 0; i < bosses.Length; i++)
            {
                bosses[i].Pause();
            }
        }
        public void OnResumeGame()
        {
            for (int i = 0; i < bosses.Length; i++)
            {
                bosses[i].Resume();
            }
        }
        public void OnBossTakeDamage(int damage, Boss boss)
        {
            currentHealth -= damage;
            healthBar.UpdateFillBar(currentHealth / totalHealth);
            if (currentHealth <= 0)
            {
                GameplayCtrl.Instance.objManager.winDoorInMap.OpenDoor();
                GameplayCtrl.Instance.enemyKill = 1;
                PlayScreenCtl playScreen = ScreenUIManager.Instance.GetScreen<PlayScreenCtl>(ScreenName.PLAYSCREEN);
                playScreen.BlockInput(true);
                playScreen.SetEnemyCount(1, 1);
                GameplayCtrl.Instance.objManager.winDoorInMap.SetTextEneMyKill(1, 1);
                GameplayCtrl.Instance.coinEarned = Random.Range(200, 500);
                CameraController.Instance.SetOrthoSize(8, 0.5f, true);
                // CameraController.Instance.SetOffset(Vector3.zero);
                CameraController.Instance.SetTargetFollow(boss.transform);
                StartCoroutine(IEOnFinishFight());
            }
        }
        IEnumerator IEOnFinishFight()
        {
            PlayScreenCtl playScreen = ScreenUIManager.Instance.GetScreen<PlayScreenCtl>(ScreenName.PLAYSCREEN);
            yield return new WaitForSeconds(2f);
            playScreen.BlockInput(false);
            yield return new WaitForSeconds(1.5f);
            CameraController.Instance.SetTargetFollow(PlayerManager.Instance.transform);
            CameraController.Instance.SetOffset(new Vector3(0, 1.6f, -10));
            CameraController.Instance.SetOrthoSize(5, 2f, false);
        }
        private void OnDrawGizmos()
        {
            if (center == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(center.position, new Vector3(arenaRangeX, arenaRangeY, 0));
        }
    }
}