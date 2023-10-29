using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyJson;
using System;

public class NotifyObject
{
    public int afterday;
    public int hour;
    public int minus;
    public int second;
    public string message;

    public string toJsonData() {
        string jsonString = "{";
        jsonString += "\"afterday\":" + afterday + ",";
        jsonString += "\"hour\":" + hour + ",";
        jsonString += "\"minus\":" + minus + ",";
        jsonString += "\"second\":" + second + ",";
        jsonString += "\"message\":\"" + message + "\"";
        jsonString += "}";

        return jsonString;
    }

    public void fromJsonData(string data) {
        var jsondata = (IDictionary<string, object>) MyJson.JsonDecoder.DecodeText(data);
        afterday = Convert.ToInt32(jsondata["afterday"]);
        hour = Convert.ToInt32(jsondata["hour"]);
        minus = Convert.ToInt32(jsondata["minus"]);
        second = Convert.ToInt32(jsondata["second"]);
        message = (string)jsondata["message"];
    }
}
