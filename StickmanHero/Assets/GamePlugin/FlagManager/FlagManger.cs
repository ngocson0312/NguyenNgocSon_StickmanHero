using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mygame.sdk
{
    public class FlagManager
    {

        private static UnityEngine.Object obSynch = new UnityEngine.Object();
        private static FlagManager _instance = null;
        public static FlagManager Instance()
        {
            lock (obSynch)
            {
                if (_instance == null)
                {
                    _instance = new FlagManager();
                    string[] arrflags = {"us"};
                    _instance.listFlagSuport.Clear();
                    for (int i = 0; i < arrflags.Length; i++)
                    {
                        _instance.listFlagSuport.Add(arrflags[i]);
                    }
                }
                return _instance;
            }
        }

        private List<string> listFlagSuport = new List<string>();

        public bool hasFlag(string contrycode)
        {
            bool re = false;
            if (contrycode != null && contrycode.Length > 0)
            {
                for (int i = 0; i < listFlagSuport.Count; i++)
                {
                    if (listFlagSuport[i].CompareTo(contrycode) == 0)
                    {
                        re = true;
                        break;
                    }
                }
            }
            return re;
        }

        public string getRanDomFlagSuport() {
            int idx = UnityEngine.Random.Range(0, listFlagSuport.Count);
            string re = listFlagSuport[idx];
            return re;
        }

        public Texture getFlagTexture(string countryCode, string defaultcode = "us") {
            Texture re = Resources.Load("Flags/" + countryCode) as Texture;
            if (re == null) {
                re = Resources.Load("Flags/" + defaultcode) as Texture;
            }
            return re;
        }
        public Sprite getFlagSprite(string countryCode, string defaultcode = "us") {
            Sprite re = Resources.Load<Sprite>("Flags/" + countryCode);
            if (re == null) {
                re = Resources.Load<Sprite>("Flags/" + defaultcode);
            }
            return re;
        }

    }
}
