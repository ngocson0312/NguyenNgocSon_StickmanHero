using System;
using System.Collections.Generic;
using UnityEngine;

namespace Myapi
{
    [Serializable]
    public class GetOtherResultOb
    {
        public List<EnemyObject> players;
        public int status;
        public string message;
    }
}
