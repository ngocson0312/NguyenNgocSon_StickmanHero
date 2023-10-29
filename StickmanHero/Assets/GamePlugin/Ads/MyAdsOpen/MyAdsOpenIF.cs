//#define TEST_ADS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace mygame.sdk
{
    public interface MyAdsOpenIF
    {
        #region MyAdsOpen implementation

        void load(string path, bool isCache);
        bool show(string path, int flagBtNo, int isFull);
        void loadAndShow(string path, bool isCache, int flagBtNo, int isFull);

        #endregion
    }
}
