#define use_load_fb

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace mygame.sdk
{
    public delegate void onLoadImageTexture(Texture2D texture);
    public delegate void onLoadImageSprite(Sprite sprite);

    public class ImageLoader : MonoBehaviour
    {
        public static ImageLoader Instance;
        private onLoadImageTexture _cbTT;
        private onLoadImageSprite _cbSP;
        private bool isloading = false;
        private List<Object4Imageloadder> listLoad;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                listLoad = new List<Object4Imageloadder>();
                _cbTT = null;
                _cbSP = null;
            }
        }

        // Use this for initialization
        //private void Start()
        //{
        //}

        // Update is called once per frame
        //private void Update()
        //{
        //}

        public static void loadImageTexture(string url, int w = 100, int h = 100, onLoadImageTexture cb = null)
        {
            Instance._loadimageTexture(url, w, h, cb);
        }

        public static void LoadImageSprite(string url, int w = 100, int h = 100, onLoadImageSprite cb = null)
        {
            Instance._loadimageSprite(url, w, h, cb);
        }

        public static void loadImageFB(string fbId, Action<Sprite> s, int w = 160, int h = 160)
        {
            Debug.Log("mysdk: loadImageFB: fbId=" + fbId);
            string urlimg = string.Format("https://graph.facebook.com/{0}/picture?width={1}&height={2}", fbId, w, h);
            LoadImageSprite(urlimg, w, h, tt => s?.Invoke(tt));
        }

        private void _loadimageTexture(string url, int w, int h, onLoadImageTexture cb)
        {
            Debug.Log("mysdk: _loadimageTexture: url=" + url);
            if (isloading)
            {
                Debug.Log("mysdk: ImageLoader: pending");
                var ob = new Object4Imageloadder();
                ob.isTexture = true;
                ob.cbTT = cb;
                ob.type = 0;
                ob.wImg = w;
                ob.hImg = h;
                ob.spRender = null;
                ob.url = url;
                listLoad.Add(ob);
            }
            else
            {
                _cbTT = cb;
                isloading = true;
                StartCoroutine(coDownloadImage(url, true, w, h));
            }
        }

        private void _loadimageSprite(string url, int w, int h, onLoadImageSprite cb)
        {
            Debug.Log("mysdk: _loadimageSprite: url=" + url);
            if (isloading)
            {
                Debug.Log("mysdk: ImageLoader: pending");
                var ob = new Object4Imageloadder();
                ob.isTexture = false;
                ob.cbSP = cb;
                ob.type = 0;
                ob.wImg = w;
                ob.hImg = h;
                ob.spRender = null;
                ob.url = url;
                listLoad.Add(ob);
            }
            else
            {
                _cbSP = cb;
                isloading = true;
                StartCoroutine(coDownloadImage(url, false, w, h));
            }
        }

        public static IEnumerator LoadSprite(string path, string name, int width, int height, Action<Sprite> action)
        {
            Debug.Log("mysdk: LoadSprite: path=" + path + ", name=" + name);
            if (name != "")
            {
                var tex = new Texture2D(width, height);
                if (!File.Exists(Application.persistentDataPath + "/" + name + ".png")) yield break;
                var fileData = File.ReadAllBytes(Application.persistentDataPath + "/" + name + ".png");
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                action?.Invoke(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero));
                yield break;
            }

            if (path.ToLower().Contains("http://") || path.ToLower().Contains("https://") || path.ToLower().Contains("www"))
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://www.my-server.com/image.png");
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success) yield break;
                Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                Texture2D tt2d = (Texture2D)myTexture;
                action?.Invoke(Sprite.Create(tt2d, new Rect(0, 0, tt2d.width, tt2d.height),
                    Vector2.zero));
                SaveTexture(tt2d, name);
            }
        }

        public static void LoadSprite(string name, int width, int height, Action<Sprite> action)
        {
            Debug.Log("mysdk: LoadSprite: name" + name);
            var tex = new Texture2D(width, height);
            if (!File.Exists(Application.persistentDataPath + "/" + name + ".png")) return;
            var fileData = File.ReadAllBytes(Application.persistentDataPath + "/" + name + ".png");
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            action?.Invoke(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero));
        }

        private IEnumerator coDownloadImage(string imageUrl, bool isTexture, int w = 100, int h = 100)
        {
            Debug.Log("mysdk: coDownloadImage: imageUrl=" + imageUrl);
            string pathimg = url2nameData(imageUrl, 1);
            Texture2D tt = null;
            if (File.Exists(DownLoadUtil.pathCache() + "/" + pathimg))
            {
                isloading = false;
                if (isTexture)
                {
                    if (_cbTT != null)
                    {
                        tt = Instance.loadTexttureFromCache(pathimg, w, h);
                        _cbTT.Invoke(tt);
                    }

                }
                else
                {
                    if (_cbSP != null)
                    {
                        tt = Instance.loadTexttureFromCache(pathimg, w, h);
                        _cbSP.Invoke(CreateSprite(tt));
                    }
                }
            }
            else
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
                yield return www.SendWebRequest();

                isloading = false;
                if (www.result == UnityWebRequest.Result.Success)
                {
                    Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    Texture2D tt2d = (Texture2D)myTexture;
                    if (tt2d != null)
                    {
                        Debug.Log("mysdk: imageLoader: Success");
                        if (isTexture)
                        {
                            _cbTT?.Invoke(tt2d);
                        }
                        else
                        {
                            _cbSP?.Invoke(CreateSprite(tt2d));
                        }
                        saveTextturetocache(tt2d, pathimg);
                    }
                    else
                    {
                        Debug.Log("mysdk: imageLoader: err");
                    }
                }
                else
                {
                    Debug.Log("mysdk: imageLoader: imageUrl err=" + www.error);
                }
            }
            if (listLoad.Count > 0)
            {
                Object4Imageloadder ob = listLoad[0];
                listLoad.RemoveAt(0);
                if (ob.isTexture)
                {
                    loadImageTexture(ob.url, ob.wImg, ob.hImg, ob.cbTT);
                }
                else
                {
                    LoadImageSprite(ob.url, ob.wImg, ob.hImg, ob.cbSP);
                }
            }
        }

        private void saveTextturetocache(Texture2D texture, string name)
        {
            if (texture == null) return;
            try
            {
                DownLoadUtil.checkDirecCache();
                texture.filterMode = FilterMode.Point;
                texture.wrapMode = TextureWrapMode.Clamp;
                texture.Apply();
                var bytes = texture.EncodeToPNG();
                File.WriteAllBytes(DownLoadUtil.pathCache() + "/" + name, bytes);
            }
            catch (Exception ex)
            {
                Debug.LogError("mysdk: ImageLoader-saveTextturetocache err=" + ex.ToString());
            }
        }

        public Texture2D loadTexttureFromCache(string name, int w, int h)
        {
            bool ise = File.Exists(DownLoadUtil.pathCache() + "/" + name);
            if (ise)
            {
                Texture2D texture = new Texture2D(w, h);
                texture.LoadImage(FileHandler.readBytes(DownLoadUtil.pathCache() + "/" + name));
                return texture;
            }
            else
            {
                return null;
            }
        }

        public static void SaveTexture(Texture2D texture, string name)
        {
            if (texture == null) return;
            try
            {
                texture.filterMode = FilterMode.Point;
                texture.wrapMode = TextureWrapMode.Clamp;
                texture.Apply();
                var bytes = texture.EncodeToPNG();
                File.WriteAllBytes(Application.persistentDataPath + "/" + name + ".png", bytes);
            }
            catch (Exception ex)
            {
                Debug.LogError("mysdk: ImageLoader-SaveTexture err=" + ex.ToString());
            }
        }

        public static Sprite CreateSprite(Texture2D texture)
        {
            return texture != null
                ? Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero)
                : null;
        }
        //0-text, 1-img, 2-data
        public static string url2nameData(string url, int typeData)
        {
            int idx = url.LastIndexOf('.');
            string news;
            string exten = "";
            if (idx > (url.Length - 6) && idx < (url.Length - 2))
            {
                news = url.Substring(0, idx);
                exten = url.Substring(idx);
            }
            else
            {
                news = url;
                if (typeData == 0)
                {//text
                    exten = ".txt";
                }
                else if (typeData == 1)
                {//image
                    exten = ".jpg";
                }
                else
                {//data
                    exten = ".dat";
                }
            }
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(news);
            string pathimg = System.Convert.ToBase64String(plainTextBytes);

            while (pathimg.Contains("/"))
            {
                pathimg = pathimg.Replace("/", "_");
            }

            return (pathimg + exten);
        }
    }

    public class Object4Imageloadder
    {
        public int type { get; set; }
        public string url { get; set; }
        public Image img { get; set; }
        public SpriteRenderer spRender { get; set; }
        public onLoadImageTexture cbTT { get; set; }
        public onLoadImageSprite cbSP { get; set; }
        public bool isTexture { get; set; }
        public int wImg { get; set; }
        public int hImg { get; set; }
    }
}