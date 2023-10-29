using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame.sdk;

namespace SuperFight
{
    public enum TypeThemes
    {
        Theme_Jungle,
        Theme_InGround,
        Theme_Hell,
        Theme_Cave,
    }

    public class StandingCtrl : MonoBehaviour
    {
        [SerializeField] GameObject[] StandingThemes;
        [SerializeField] GameObject[] StandingThemesIOS;
        [SerializeField] Transform StandingAnchor;
        [SerializeField] GameObject CurrentStanding;
        private void Start()
        {
            StandingAnchor = transform;
            ShowStanding(PlayerPrefsUtil.Theme);
        }


        public void ShowStanding(int _idTheme)
        {
            if (!StandingAnchor.Find(StandingThemes[_idTheme].name + "(Clone)"))
            {
                if (CurrentStanding != null)
                {
                    CurrentStanding.SetActive(false);
                }
                CurrentStanding = Instantiate(StandingThemes[_idTheme], StandingAnchor);
            }
            CurrentStanding.SetActive(true);
        }
        public void HideStanding()
        {
            CurrentStanding.SetActive(false);
        }

        public void SetIdTheme(TypeThemes _typeThemes)
        {
            switch (_typeThemes)
            {
                case TypeThemes.Theme_Jungle:
                    PlayerPrefsUtil.Theme = 0;
                    break;
                case TypeThemes.Theme_InGround:
                    PlayerPrefsUtil.Theme = 1;
                    break;
                case TypeThemes.Theme_Hell:
                    PlayerPrefsUtil.Theme = 2;
                    break;
                case TypeThemes.Theme_Cave:
                    PlayerPrefsUtil.Theme = 3;
                    break;
                default:
                    break;
            }

        }

    }
}
