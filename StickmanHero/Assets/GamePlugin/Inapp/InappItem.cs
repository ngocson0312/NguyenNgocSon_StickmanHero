using System;
using UnityEngine;
using System.Collections.Generic;
#if ENABLE_INAPP
using UnityEngine.Purchasing;

#endif

namespace mygame.sdk
{
    public class PurchaseCallback
    {
        public int status;
        public string skuid;

        public PurchaseCallback(int st, string sid)
        {
            status = st;
            skuid = sid;
        }
    }

    public class PkgObject
    {
        public string id;
        public string sku;
        public int typeInapp; //0-consum, 1-nonconsum, 2-sub
        public int periodSub = 0;
        public string price = "";
        public int memPrice = 0;
        public int countRcvDaily = 0;
        public int dayPurchased = 0;
        public int dayRcvFakeSub = 0;
        public InappRcvObject rcv = new InappRcvObject();
        public InappRcvObject dailyRcv = new InappRcvObject();

        public PkgObject()
        {
        }
    }

    public class InappCountryOb
    {
        public string countryCode;
        public Dictionary<string, PkgObject> listWithId;
        public Dictionary<string, PkgObject> listWithSku;

        public InappCountryOb(string code)
        {
            countryCode = code;
            listWithId = new Dictionary<string, PkgObject>();
            listWithSku = new Dictionary<string, PkgObject>();
        }
    }

    public class InappRcvItemsObject
    {
        public List<int> listValue = new List<int>();
    }

    public class InappRcvObject
    {
        public int vipPoint = 0;
        public int removeads = 0;

        public string money = ""; //gold:100,crystal:300
        public string items = ""; //boom:10,shield:20 -> rcv 10 boom, 20 shield
        public string equipments = "";//gun:1#2,hat:1#2 gun with id: 1,2; hat with id 1,2

        private Dictionary<string, int> dicMoneys = new Dictionary<string, int>();
        private Dictionary<string, int> dicItems = new Dictionary<string, int>();
        private Dictionary<string, InappRcvItemsObject> dicEquipments = new Dictionary<string, InappRcvItemsObject>();

        public InappRcvObject()
        {
            removeads = 0;
        }

        public int countAllItemRcv()
        {
            int re = 0;
            if (money.Length > 2)
            {
                string[] arrm = money.Split(new char[',']);
                re += arrm.Length;
            }

            if (items.Length > 2)
            {
                string[] arrm = items.Split(new char[',']);
                re += arrm.Length;
            }

            return re;
        }

        public Dictionary<string, int> getMoneys()
        {
            if (dicMoneys.Count <= 0)
            {
                string[] vs = money.Split(new char[] { ',' });
                for (int i = 0; i < vs.Length; i++)
                {
                    string[] vsitem = vs[i].Split(new char[] { ':' });
                    if (vsitem.Length == 2)
                    {
                        int va = int.Parse(vsitem[1]);
                        dicMoneys.Add(vsitem[0], va);
                    }
                }
            }
            return dicMoneys;
        }

        public int getMoney(string key)
        {
            getMoneys();
            if (key != null && key.Length > 0 && dicMoneys.ContainsKey(key))
            {
                return dicMoneys[key];
            }

            return 0;
        }

        public Dictionary<string, int> getItems()
        {
            if (dicItems.Count <= 0)
            {
                try
                {
                    string[] vs = items.Split(new char[] { ',' });
                    for (int i = 0; i < vs.Length; i++)
                    {
                        string[] vsitem = vs[i].Split(new char[] { ':' });
                        if (vsitem.Length == 2)
                        {
                            int va = int.Parse(vsitem[1]);
                            dicItems.Add(vsitem[0], va);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("mysdk: ex=" + ex.ToString());
                }
            }

            return dicItems;
        }

        public int getItem(string key)
        {
            getItems();
            if (key != null && key.Length > 0 && dicItems.ContainsKey(key))
            {
                return dicItems[key];
            }
            else
            {
                return 0;
            }
        }

        public Dictionary<string, InappRcvItemsObject> getEquipments()
        {
            if (dicEquipments.Count <= 0)
            {
                try
                {
                    string[] vs = equipments.Split(new char[] { ',' });
                    for (int i = 0; i < vs.Length; i++)
                    {
                        string[] vsitem = vs[i].Split(new char[] { ':' });
                        if (vsitem.Length == 2)
                        {
                            string[] litem = vsitem[1].Split(new char[] { '#' });
                            InappRcvItemsObject ova = new InappRcvItemsObject();
                            for (int ii = 0; ii < litem.Length; ii++)
                            {
                                int iva = int.Parse(litem[ii]);
                                ova.listValue.Add(iva);
                            }
                            dicEquipments.Add(vsitem[0], ova);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("mysdk: ex=" + ex.ToString());
                }
            }
            return dicEquipments;
        }

        public InappRcvItemsObject getEquipment(string key)
        {
            getEquipments();
            if (key != null && key.Length > 0 && dicEquipments.ContainsKey(key))
            {
                return dicEquipments[key];
            }
            else
            {
                return null;
            }
        }
    }
}