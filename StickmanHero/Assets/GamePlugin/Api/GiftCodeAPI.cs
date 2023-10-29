using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
using mygame.sdk;

namespace Myapi
{
    public class GiftCodeAPI : SingleTonApi<GiftCodeAPI, GiftCodeOb>
    {
        public void check(string code, Action<GiftCodeOb> cb)
        {
            long idrq = mygame.sdk.SdkUtil.systemCurrentMiliseconds();
            idrq = addQueueCallback(idrq, cb);
            PostRequest(idrq, "");
        }

        protected override void OnError(long idRequest, string error)
        {
            base.OnError(idRequest, error);
            if (callBacks.ContainsKey(idRequest) && callBacks[idRequest] != null)
            {
                callBacks[idRequest](null);
                callBacks.Remove(idRequest);
            }
        }

        protected override void Process(long idRequest, GiftCodeOb data)
        {
            if (callBacks.ContainsKey(idRequest) && callBacks[idRequest] != null)
            {
                callBacks[idRequest](data);
                callBacks.Remove(idRequest);
            }
        }
    }
}
