using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using mygame.sdk;

public class InappUtil
{
    public static void parserDataSkus(string data, IDictionary<string, InappCountryOb> listCountry)
    {
        if (data == null || listCountry == null)
        {
            return;
        }

        listCountry.Clear();

        var listinapp = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(data);
        if (listinapp != null || listinapp.Count > 0)
        {
            foreach (KeyValuePair<string, object> itemdata in listinapp)
            {
                InappCountryOb countryInapp = new InappCountryOb(itemdata.Key);
                listCountry.Add(itemdata.Key, countryInapp);
                IDictionary<string, object> childs = (IDictionary<string, object>)itemdata.Value;
                foreach (KeyValuePair<string, object> typesku in childs)
                {
                    if (typesku.Key.Equals("sub"))
                    {
                        IList<object> subList = (IList<object>)typesku.Value;
                        foreach (var subchild in subList)
                        {
                            IDictionary<string, object> sublistitem = (IDictionary<string, object>)subchild;
                            PkgObject inappsub = new PkgObject();
                            inappsub.id = (string)sublistitem["id"];
                            inappsub.sku = (string)sublistitem["pkg"];
                            inappsub.typeInapp = 2;
                            inappsub.price = (string)sublistitem["price"];
                            if (sublistitem.ContainsKey("period"))
                            {
                                inappsub.periodSub = Convert.ToInt32(sublistitem["period"]);
                            }
                            if (sublistitem.ContainsKey("mem_price"))
                            {
                                inappsub.memPrice = Convert.ToInt32(sublistitem["mem_price"]);
                                if (inappsub.memPrice == 1)
                                {
                                    inappsub.price = PlayerPrefs.GetString(inappsub.sku, inappsub.price);
                                }
                            }
                            getReceiver(sublistitem, inappsub);
                            countryInapp.listWithId.Add(inappsub.id, inappsub);
                            countryInapp.listWithSku.Add(inappsub.sku, inappsub);
                        }
                    }
                    else if (typesku.Key.Equals("consum"))
                    {
                        IList<object> consumList = (IList<object>)typesku.Value;
                        foreach (var consumechild in consumList)
                        {
                            IDictionary<string, object> consumlistitem = (IDictionary<string, object>)consumechild;
                            PkgObject inappcon = new PkgObject();
                            inappcon.id = (string)consumlistitem["id"];
                            inappcon.sku = (string)consumlistitem["pkg"];
                            inappcon.typeInapp = 0;
                            inappcon.price = (string)consumlistitem["price"];
                            if (consumlistitem.ContainsKey("period"))
                            {
                                inappcon.periodSub = Convert.ToInt32(consumlistitem["period"]);
                                inappcon.countRcvDaily = PlayerPrefsBase.Instance().getInt($"iap_{inappcon.id}_dayrcv", 0);
                                inappcon.dayPurchased = PlayerPrefsBase.Instance().getInt($"iap_{inappcon.id}_daypur", 0);
                                inappcon.dayRcvFakeSub = PlayerPrefsBase.Instance().getInt($"iap_{inappcon.id}_dayrcvpur", 0);
                            }
                            if (consumlistitem.ContainsKey("mem_price"))
                            {
                                inappcon.memPrice = Convert.ToInt32(consumlistitem["mem_price"]);
                                if (inappcon.memPrice == 1)
                                {
                                    inappcon.price = PlayerPrefs.GetString(inappcon.sku, inappcon.price);
                                }
                            }
                            getReceiver(consumlistitem, inappcon);
                            countryInapp.listWithId.Add(inappcon.id, inappcon);
                            countryInapp.listWithSku.Add(inappcon.sku, inappcon);
                        }
                    }
                    else if (typesku.Key.Equals("non_consum"))
                    {
                        IList<object> consumList = (IList<object>)typesku.Value;
                        foreach (var consumechild in consumList)
                        {
                            IDictionary<string, object> consumlistitem = (IDictionary<string, object>)consumechild;
                            PkgObject inappcon = new PkgObject();
                            inappcon.id = (string)consumlistitem["id"];
                            inappcon.sku = (string)consumlistitem["pkg"];
                            inappcon.typeInapp = 1;
                            inappcon.price = (string)consumlistitem["price"];
                            if (consumlistitem.ContainsKey("mem_price"))
                            {
                                inappcon.memPrice = Convert.ToInt32(consumlistitem["mem_price"]);
                                if (inappcon.memPrice == 1)
                                {
                                    inappcon.price = PlayerPrefs.GetString(inappcon.sku, inappcon.price);
                                }
                            }
                            getReceiver(consumlistitem, inappcon);
                            countryInapp.listWithId.Add(inappcon.id, inappcon);
                            countryInapp.listWithSku.Add(inappcon.sku, inappcon);
                        }
                    }
                }
            }
        }
    }

    public static void getReceiver(IDictionary<string, object> data, PkgObject ob)
    {
        if (data.ContainsKey("receiver"))
        {
            IDictionary<string, object> rcvlist = (IDictionary<string, object>)data["receiver"];
            foreach (var itemrcv in rcvlist)
            {
                FieldInfo info = ob.rcv.GetType().GetField(itemrcv.Key);
                if (info != null)
                {
                    info.SetValue(ob.rcv, Convert.ChangeType(itemrcv.Value, info.FieldType));
                }
            }
        }
    }
}