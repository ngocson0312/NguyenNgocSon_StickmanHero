using System;
using System.Collections.Generic;
using UnityEngine;

namespace Myapi
{
    public class LogOb
    {
        public int idlog;
        public MyEventLog eventLog;
        public Dictionary<string, string> param;
        public string data;
        public int countTry = 0;
        

        public LogOb(int idl, MyEventLog e, Dictionary<string, string> p, string d) {
            idlog = idl;
            eventLog = e;
            param = p;
            data = d;
            countTry = 0;
        }

        public LogOb(string datalog) {
            fromDatalog(datalog);
        }

        public void fromDatalog(string datalog) {
            string[] sl = datalog.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (sl != null && sl.Length >= 3) {
                idlog = int.Parse(sl[0]);
                eventLog = (MyEventLog)Enum.Parse(typeof(MyEventLog), sl[1], true);
                countTry = int.Parse(sl[2]);
                if (sl.Length > 3) {
                    for(int i = 3; i < sl.Length; i++) {
                        if (sl[i].StartsWith("p:")) {
                            string sp = sl[i].Substring(2);
                            param = new Dictionary<string, string>();
                            string[] arrp = datalog.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string ip in arrp) {
                                string[] evev = ip.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                                if (evev != null && evev.Length == 2) {
                                    param.Add(evev[0], evev[1]);
                                }
                            }
                        } else if (sl[i].StartsWith("d:")) {
                            data = sl[i].Substring(2);
                        }
                    }
                }
            }
        }

        public string todatalog() {
            string eventname = eventLog.ToString();
            string re = $"{idlog},{eventname},{countTry}";
            if (param != null && param.Count > 0) {
                re += ",p:";
                bool isbegin = true;
                foreach (KeyValuePair<string, string> entry in param)
                {
                    if (isbegin) {
                        isbegin = false;
                        re += ($"{entry.Key}={entry.Value}");
                    } else {
                        re += ($"&{entry.Key}={entry.Value}");
                    }
                }
            }
            if (data != null && data.Length > 0) {
                re += $"d:{data}";
            }

            return re;
        }
    }
}
