using System.Collections.Generic;
using System;

namespace Myapi
{
    [Serializable]
    public class GiftCodeOb
    {
        public string message;
        public List<GiftCodeRewardOb> rewards;
        public int status;

        public void RcvReward()
        {
            if (rewards != null)
            {
                List<int> listIdrw = new List<int>();
                List<int> listvarw = new List<int>();
                foreach (var reward in rewards)
                {
                    
                }
            }
        }
    }

    [Serializable]
    public class GiftCodeRewardOb
    {
        public int amount;
        public int type;
    }
}

