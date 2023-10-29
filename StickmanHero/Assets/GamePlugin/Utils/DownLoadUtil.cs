using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;

namespace mygame.sdk
{
    public delegate void onDownloadFile(int state);
    class DownLoadUtil : MonoBehaviour
    {
        public static DownLoadUtil Instance;
        private onDownloadFile _cb;
        private bool isloading = false;
        private List<Object4DownloadUtil> listLoad = new List<Object4DownloadUtil>();


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _cb = null;
            }
        }

        public static void downloadText(string url, string path, onDownloadFile cb = null)
        {
            Debug.Log("mysdk: downloadText=" + url);
            if (Instance == null)
            {
                return;
            }
            bool isDown = false;
            if (path == null || path.Length < 3)
            {
                string name = ImageLoader.url2nameData(url, 0);
                path = pathCache() + "/" + name;
                if (File.Exists(path))
                {
                    isDown = true;
                    cb?.Invoke(1);
                }
            }
            if (!isDown)
            {
                Instance.doDownloadData(url, path, 0, cb);
            }
        }

        public static void downloadImg(string url, string path, onDownloadFile cb = null)
        {
            Debug.Log("mysdk: downloadImg=" + url);
            if (Instance == null)
            {
                return;
            }
            bool isDown = false;
            if (path == null || path.Length < 3)
            {
                string name = ImageLoader.url2nameData(url, 1);
                path = pathCache() + "/" + name;
                if (File.Exists(path))
                {
                    isDown = true;
                    cb?.Invoke(1);
                }
            }
            if (!isDown)
            {
                Instance.doDownloadData(url, path, 1, cb);
            }
        }

        public static void downloadData(string url, string path, onDownloadFile cb = null)
        {
            Debug.Log("mysdk: downloadData=" + url);
            if (Instance == null)
            {
                return;
            }
            bool isDown = false;
            if (path == null || path.Length < 3)
            {
                string name = ImageLoader.url2nameData(url, 2);
                path = pathCache() + "/" + name;
                if (File.Exists(path))
                {
                    isDown = true;
                    cb?.Invoke(1);
                }
            }
            if (!isDown)
            {
                Instance.doDownloadData(url, path, 2, cb);
            }
        }

        public static string pathCache()
        {
            return Application.persistentDataPath + "/caches";
        }

        public static void checkDirecCache()
        {
            try
            {
                string path = pathCache() + "/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (IOException ex)
            {
                Debug.Log("mysdk: ex=" + ex.ToString());
            }
        }

        void doDownloadData(string url, string path, int typeData, onDownloadFile cb = null)
        {
            if (isloading)
            {
                Debug.Log("mysdk: DownLoadUtil-doDownloadData: pending");
                var ob = new Object4DownloadUtil();
                ob.cb = cb;
                ob.path = path;
                ob.url = url;
                ob.typeData = typeData;
                listLoad.Add(ob);
            }
            else
            {
                _cb = cb;
                isloading = true;
                StartCoroutine(coDownload(url, path, typeData, cb));
            }
        }

        private IEnumerator coDownload(string url, string path, int typeData, onDownloadFile cb = null)
        {
            Debug.Log("mysdk: DownLoadUtil=" + url + ", type=" + typeData);
            UnityWebRequest uwr = UnityWebRequest.Get(url);
            yield return uwr.SendWebRequest();
            isloading = false;
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                try
                {
                    checkDirecCache();
                    if (typeData == 0)
                    {
                        File.WriteAllText(path, uwr.downloadHandler.text);
                    }
                    else if (typeData == 1)
                    {
                        File.WriteAllBytes(path, uwr.downloadHandler.data);
                    }
                    else
                    {
                        File.WriteAllBytes(path, uwr.downloadHandler.data);
                    }
                    Debug.Log("mysdk: DownLoadUtil done 1");
                    _cb?.Invoke(1);
                }
                catch (Exception ex)
                {
                    Debug.Log("mysdk: DownLoadUtil err write file err=" + typeData);
                    Debug.Log("mysdk: DownLoadUtil err write file err=" + ex.ToString());
                    _cb?.Invoke(-1);
                }
            }
            else
            {
                Debug.Log("mysdk: DownLoadUtil err=" + uwr.error.ToString());
                _cb?.Invoke(-1);
            }
            if (listLoad.Count > 0)
            {
                Object4DownloadUtil ob = listLoad[0];
                listLoad.RemoveAt(0);
                doDownloadData(ob.url, ob.path, ob.typeData, ob.cb);
            }
        }
    }
    public class Object4DownloadUtil
    {
        public string url { get; set; }
        public string path { get; set; }
        public int typeData { get; set; }
        public onDownloadFile cb { get; set; }
    }
}
