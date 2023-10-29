using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using mygame.sdk;

namespace Myapi
{
    public class ApiManager : MonoBehaviour
    {
        public static ApiManager Instance;
        private bool isLoading = false;
        //for game
        public const string baseAppUrl = "https://gangmaster.club/";
        public const string API_GIFT_CODE = "checkgiftcode?";

        public List<BaseApi> listApi { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                isLoading = false;
                listApi = new List<BaseApi>();
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        public T checkApi<T>() where T : BaseApi
        {
            for (int i = 0; i < listApi.Count; i++)
            {
                if (isOfType<T>(listApi[i]))
                {
                    return (T)listApi[i];
                }
            }

            return null;
        }

        private bool isOfType<T>(object va)
        {
            return va is T;
        }

        public void getTimeOnline(Action<bool, long> result)
        {
            StartCoroutine(_getTimeOnline(result));
        }

        public void getRequest(string urlget, Action<bool, string> result = null)
        {
            StartCoroutine(_getRequest(urlget, result));
        }

        public void postRequest(string urlpost, Action<bool, string> result = null)
        {
            StartCoroutine(_postRequest(urlpost, "", result));
        }

        public void postRequest(string urlpost, string data, Action<bool, string> result = null)
        {
            StartCoroutine(_postRequest(urlpost, data, result));
        }

        public void postRequest(string urlpost, byte[] bytes, Action<bool, string> result = null)
        {
            StartCoroutine(_postRequest(urlpost, bytes, result));
        }
        public void postRequest(string urlpost, Dictionary<string, string> field, Dictionary<string, string> dicfiles, Action<bool, string> result = null)
        {
            StartCoroutine(_postRequest(urlpost, field, dicfiles, result));
        }

        private IEnumerator _getRequest(string url, Action<bool, string> result)
        {
            Debug.Log("mysdk: get request=" + url);
            UnityWebRequest uwr = UnityWebRequest.Get(url);
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error + "=" + uwr.error);
                result?.Invoke(false, uwr.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                result?.Invoke(true, uwr.downloadHandler.text);
            }
        }

        private IEnumerator _postRequest(string url, string data, Action<bool, string> result)
        {
            Debug.Log("mysdk: postRequest=" + url);
            UnityWebRequest uwr = UnityWebRequest.Post(url, data);
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error + "=" + uwr.error);
                result?.Invoke(false, uwr.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                result?.Invoke(true, uwr.downloadHandler.text);
            }
        }

        private IEnumerator _postRequest(string url, byte[] data, Action<bool, string> result)
        {
            Debug.Log("mysdk: postRequest=" + url);
            UnityWebRequest uwr = new UnityWebRequest(url, "POST")
            {
                uploadHandler = new UploadHandlerRaw(data),
                downloadHandler = new DownloadHandlerBuffer()
            };
            uwr.SetRequestHeader("Content-Type", "application/json");
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error + "=" + uwr.error);
                result?.Invoke(false, uwr.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                result?.Invoke(true, uwr.downloadHandler.text);
            }
        }

        private IEnumerator _postRequest(string url, Dictionary<string, string> fields, Dictionary<string, string> dicfiles, Action<bool, string> result)
        {
            Debug.Log("mysdk: postRequest=" + url);
            WWWForm form = new WWWForm();
            if (fields != null)
            {
                foreach (var item in fields)
                {
                    form.AddField(item.Key, item.Value);
                }
            }

            if (dicfiles != null)
            {
                int idx = 0;
                foreach (var item in dicfiles)
                {
                    byte[] bpush = System.IO.File.ReadAllBytes(item.Value);
                    form.AddBinaryData(item.Key, bpush, Path.GetFileName(item.Value));
                    idx++;
                }
            }
            UnityWebRequest uwr = UnityWebRequest.Post(url, form);
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("uwr.error=" + uwr.error);
                result?.Invoke(false, uwr.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                result?.Invoke(true, uwr.downloadHandler.text);
            }
        }

        private IEnumerator _getTimeOnline(Action<bool, long> result)
        {
            UnityWebRequest www = new UnityWebRequest("http://www.google.com");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                result?.Invoke(false, -1);
                yield break;
            }
            else
            {
                string date = www.GetResponseHeader("date");
                var dd = DateTime.ParseExact(date, "ddd, dd MMM yyyy HH:mm:ss 'GMT'"
                    , System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat
                    , System.Globalization.DateTimeStyles.AssumeUniversal);
                result?.Invoke(true, (dd.Ticks - 621355968000000000) / 10000);
            }
        }
    }
}
