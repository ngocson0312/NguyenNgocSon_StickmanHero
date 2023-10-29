using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Myapi
{
    public class SingleTonApi<T, TObj> : BaseApi where T : BaseApi
    {
        private static T _instance = null;
        private long idCurrMax = -1;
        protected Dictionary<long, Action<TObj>> callBacks = new Dictionary<long, Action<TObj>>();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = ApiManager.Instance.checkApi<T>();
                    if (_instance == null)
                    {
                        _instance = (T)Activator.CreateInstance(typeof(T));
                    }
                }

                return _instance;
            }
        }

        protected override void onLoadOk(long idRequest, string data)
        {
            try
            {
                Process(idRequest, JsonConvert.DeserializeObject<TObj>(data));
            }
            catch (Exception ex)
            {
                OnError(idRequest, ex.ToString());
            }
        }

        protected override void onLoadErr(long idRequest, string err)
        {
            OnError(idRequest, err);
        }

        protected virtual void OnError(long idRequest, string error)
        {
        }

        protected virtual void Process(long idRequest, TObj data)
        {
        }

        protected long addQueueCallback(long idRequest, Action<TObj> callBack)
        {
            if (callBack != null)
            {
                if (idCurrMax < 0)
                {
                    idCurrMax = idRequest;
                }
                else
                {
                    if (idRequest <= idCurrMax)
                    {
                        idRequest = idCurrMax + 1;
                    }
                    idCurrMax = idRequest;
                }
                callBacks.Add(idRequest, callBack);
            }
            return idRequest;
        }
    }
}