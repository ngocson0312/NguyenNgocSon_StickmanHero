using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace mygame.sdk
{
    public class UpdateGameController : MonoBehaviour
    {
        public Text txtTitle;
        public Text txtDes;
        public GameObject btClose;

        private int statusUpdate;
        private string gameidUpdate;
        private string linkUpdate;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void onclickClose()
        {
            gameObject.SetActive(false);
            transform.parent.gameObject.SetActive(false);
        }

        public void onClickUpdate()
        {
            if (statusUpdate == 1)
            {
                onclickClose();
            }
            if (linkUpdate != null && linkUpdate.Length > 3)
            {
                GameHelper.Instance.gotoLink(linkUpdate);
            }
            else
            {
                GameHelper.Instance.gotoStore(gameidUpdate);
            }
        }

        public void showUpdate(int verup, int status, string gameid, string link = "", string title = "", string des = "")
        {
            if (verup >= AppConfig.verapp)
            {
                if (status == 2)
                {
                    btClose.SetActive(false);
                }
                else
                {
                    btClose.SetActive(true);
                }
                transform.parent.gameObject.SetActive(true);
                gameObject.SetActive(true);
                if (title != null && title.Length > 3)
                {
                    txtTitle.text = title;
                }
                if (des != null && des.Length > 3)
                {
                    txtDes.text = des;
                }
                this.statusUpdate = status;
                this.gameidUpdate = gameid;
                this.linkUpdate = link;
            }
        }
    }
}
