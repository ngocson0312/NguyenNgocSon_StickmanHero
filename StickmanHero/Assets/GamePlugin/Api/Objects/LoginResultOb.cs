using System;
using System.Collections.Generic;
using UnityEngine;

namespace Myapi
{
    [Serializable]
    public class LoginResultOb
    {
        public int status;
        public string message;
        public int currentcoin;
        public int defendwarriorlost;
        public int coinlost;
        public int attack;
        public int world;
        public List<BaseBuildingOb> playerbuilding;
        public List<OtherAttack> listattack;
    }
}
