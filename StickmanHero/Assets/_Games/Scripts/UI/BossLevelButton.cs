using TMPro;
using UnityEngine.UI;
using UnityEngine;
namespace SuperFight
{
    public class BossLevelButton : MonoBehaviour
    {
        BossLevelPanel levelPanel;
        private int level;
        [SerializeField] TextMeshProUGUI levelText;
        [SerializeField] Image bgButton;
        [SerializeField] Sprite lockBg;
        [SerializeField] Sprite unlockBg;
        [SerializeField] Sprite selectBg;
        public void Initialize(BossLevelPanel levelPanel)
        {
            this.levelPanel = levelPanel;
            GetComponent<Button>().onClick.AddListener(SelectLevel);
        }
        public void UpdateButton(int level)
        {
            this.level = level;
            levelText.text = level.ToString();
            if (level <= DataManager.Instance.currentBossLevel)
            {
                bgButton.sprite = unlockBg;
            }
            else
            {
                bgButton.sprite = lockBg;
            }

            if (level == DataManager.Instance.currentBossLevelSelect)
            {
                bgButton.sprite = selectBg;
            }
        }
        public void OnSelectLevel()
        {
            bgButton.sprite = selectBg;
        }
        void SelectLevel()
        {
            if (level > DataManager.Instance.currentBossLevel) return;
            levelPanel.OnSelectLevel(level);
            OnSelectLevel();
        }
    }
}

