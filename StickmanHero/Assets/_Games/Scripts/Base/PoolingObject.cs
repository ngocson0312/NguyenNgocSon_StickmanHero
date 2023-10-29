using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolingObject : Singleton<PoolingObject>
{
    public static Dictionary<string, List<MonoBehaviour>> ObjPooling =
              new Dictionary<string, List<MonoBehaviour>>();


    public static Obj GetObjectFree<Obj>(Obj _obj) where Obj : MonoBehaviour
    {
        List<MonoBehaviour> listObjects;
        string type = _obj.gameObject.name;

        if (!ObjPooling.ContainsKey(type))
        {
            listObjects = new List<MonoBehaviour>
                {
                    Instantiate(_obj)
                };
            ObjPooling.Add(type, listObjects);
            Obj obj = (Obj)listObjects[0];
            return obj;
        }
        else
        {
            listObjects = ObjPooling[type];
            for (int index = 0; index < listObjects.Count; index++)
            {
                var v = listObjects[index];
                if (v == null)
                {
                    listObjects.Remove(v);
                    continue;
                }
                if (!v.gameObject.activeSelf)
                {
                    v.gameObject.SetActive(true);
                    return v as Obj;
                }
            }

            Obj obj = Instantiate(_obj);
            listObjects.Add(obj);
            ObjPooling[type] = listObjects;
            return obj;
        }
    }
    
    public static Obj GetObjectFree<Obj>(Obj _obj, Vector3 position, Quaternion rot) where Obj : MonoBehaviour
    {
        List<MonoBehaviour> listObjects;
        string type = _obj.gameObject.name;

        if (!ObjPooling.ContainsKey(type))
        {
            listObjects = new List<MonoBehaviour>
                {
                    Instantiate(_obj)
                };
            ObjPooling.Add(type, listObjects);
            Obj obj = (Obj)listObjects[0];
            obj.transform.position = position;
            obj.transform.rotation = rot;
            return obj;
        }
        else
        {
            listObjects = ObjPooling[type];
            for (int index = 0; index < listObjects.Count; index++)
            {
                var v = listObjects[index];
                if (v == null)
                {
                    listObjects.Remove(v);
                    continue;
                }
                if (!v.gameObject.activeSelf)
                {
                    v.gameObject.SetActive(true);
                    v.transform.position = position;
                    v.transform.rotation = rot;
                    return v as Obj;
                }
            }

            Obj obj = Instantiate(_obj);
            obj.transform.position = position;
            obj.transform.rotation = rot;
            listObjects.Add(obj);
            ObjPooling[type] = listObjects;
            return obj;
        }
    }

    public static void FreeObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = Instance.transform;
        obj.transform.localPosition = Vector3.zero;
        // obj.transform.localEulerAngles = Vector3.zero;
        // obj.transform.localScale = Vector3.one;
    }

    public static void FreeAllObject()
    {
        foreach (var poolList in ObjPooling)
        {
            if (poolList.Value.Count == 0)
            {
                continue;
            }
            else
            {
                foreach (var obj in poolList.Value)
                {
                    if (obj.gameObject != null)
                    {
                        FreeObject(obj.gameObject);
                    }
                }
            }
        }
    }


    public static void DestroyAllObjectPooling()
    {
        foreach (var poolList in ObjPooling)
        {
            if (poolList.Value.Count == 0)
            {
                continue;
            }
            else
            {
                foreach (var obj in poolList.Value)
                {
                    Destroy(obj.gameObject);
                }
            }
        }
        ObjPooling.Clear();
    }
}
