using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Myapi
{
    public abstract class BaseApi
    {
        private bool isLogContent = true;

        public void GetRequest(long idRequest, string url)
        {
            ApiManager.Instance.getRequest(url, (b, s) =>
            {
                if (b)
                {
                    onLoadOk(idRequest, s);
                }
                else
                {
                    onLoadErr(idRequest, s);
                }

                if (isLogContent)
                {
                    Debug.Log("mysdk: apicontent=" + s);
                }
            });
        }

        public void PostRequest(long idRequest, string url)
        {
            ApiManager.Instance.postRequest(url, (b, s) =>
            {
                if (b)
                {
                    onLoadOk(idRequest, s);
                }
                else
                {
                    onLoadErr(idRequest, s);
                }

                if (isLogContent)
                {
                    Debug.Log("mysdk: apicontent=" + s);
                }
            });
        }

        public void PostRequest(long idRequest, string url, string data)
        {
            ApiManager.Instance.postRequest(url, data, (b, s) =>
            {
                if (b)
                {
                    onLoadOk(idRequest, s);
                }
                else
                {
                    onLoadErr(idRequest, s);
                }

                if (isLogContent)
                {
                    Debug.Log("mysdk: apicontent=" + s);
                }
            });
        }

        public void PostRequest(long idRequest, string url, byte[] bytes)
        {
            ApiManager.Instance.postRequest(url, bytes, (b, s) =>
            {
                if (b)
                {
                    onLoadOk(idRequest, s);
                }
                else
                {

                    onLoadErr(idRequest, s);
                }

                if (isLogContent)
                {
                    Debug.Log("mysdk: apicontent=" + s);
                }
            });
        }

        public void PostRequest(long idRequest, string url, Dictionary<string, string> field = null, Dictionary<string, string> dicfiles = null)
        {
            ApiManager.Instance.postRequest(url, field, dicfiles, (b, s) =>
            {
                if (b)
                {
                    onLoadOk(idRequest, s);
                }
                else
                {
                    onLoadErr(idRequest, s);
                }

                if (isLogContent)
                {
                    Debug.Log("mysdk: apicontent=" + s);
                }
            });
        }

        protected abstract void onLoadOk(long idRequest, string data);

        protected abstract void onLoadErr(long idRequest, string err);
    }
}
