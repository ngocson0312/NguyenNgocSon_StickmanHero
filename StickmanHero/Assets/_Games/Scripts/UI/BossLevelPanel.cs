using System.Collections;
using System.Collections.Generic;
using mygame.sdk;
using UnityEngine;
using UnityEngine.UI;
namespace SuperFight
{
    public class BossLevelPanel : MonoBehaviour
    {
        public BossLevelButton[] levelButtons;
        [SerializeField] Button closePanelBtn;
        [SerializeField] Button nextPageBtn;
        [SerializeField] Button previousPageBtn;
        private int currentPage;
        public void Initialize()
        {
            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i].Initialize(this);
            }
            closePanelBtn.onClick.AddListener(Close);
            nextPageBtn.onClick.AddListener(OnClickNext);
            previousPageBtn.onClick.AddListener(OnClickPrevious);
            UpdatePage();
        }
        void OnClickNext()
        {
            currentPage++;
            UpdatePage();
        }
        void OnClickPrevious()
        {
            currentPage--;
            UpdatePage();
        }
        void UpdatePage()
        {
            previousPageBtn.gameObject.SetActive(true);
            nextPageBtn.gameObject.SetActive(true);
            int currentLevel = GameRes.GetLevel() / 5;
            if (currentLevel < 2)
            {
                currentLevel = 2;
            }
            if(currentLevel > 5)
            {
                currentLevel = 5;
            }
            DataManager.Instance.currentBossLevel = currentLevel;
            float maxPage = (float)currentLevel / (float)levelButtons.Length;
            maxPage = Mathf.FloorToInt(maxPage);

            if (currentPage == 0)
            {
                previousPageBtn.gameObject.SetActive(false);
            }
            if (currentPage == maxPage)
            {
                nextPageBtn.gameObject.SetActive(false);
            }
            for (int i = 0; i < levelButtons.Length; i++)
            {
                int newLevel = (levelButtons.Length * currentPage) + i + 1;
                levelButtons[i].UpdateButton(newLevel);
            }
        }
        public void OnSelectLevel(int level)
        {
            DataManager.Instance.currentBossLevelSelect = level;
            GameManager.Instance.PlayLevelBoss(level);
            Close();
        }
        public void Close()
        {
            gameObject.SetActive(false);
        }
        public void Open()
        {
            gameObject.SetActive(true);
            UpdatePage();
        }
    }
}

