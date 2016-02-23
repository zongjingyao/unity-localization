using MiniJSON;
using System;
using System.Collections.Generic;
public class JsonHelper
{
    private JsonHelper() { }

    public static object OptObj(Dictionary<string, object> dic, string key)
    {
        if (dic.ContainsKey(key))
        {
            return dic[key];
        }
        else
        {
            return new object();
        }
    }

    public static List<object> OptList(Dictionary<string, object> dic, string key)
    {
        if (dic.ContainsKey(key))
        {
            return (List<object>)dic[key];
        }
        else
        {
            List<object> list = new List<object>();
            return list;
        }
    }

    public static bool OptBool(Dictionary<string, object> dic, string key, bool fallback = false)
    {
        if (dic.ContainsKey(key))
        {
            return (bool)dic[key];
        }
        else
        {
            return fallback;
        }
    }

    public static int OptInt(Dictionary<string, object> dic, string key, int fallback = 0)
    {
        if (!dic.ContainsKey(key))
        {
            return fallback;
        }

        if (dic[key].GetType() == typeof(int))
        {
            return (int)dic[key];
        }
        else if (dic[key].GetType() == typeof(long))
        {
            return (int)OptLong(dic, key, fallback);
        }
        else
        {
            throw new Exception("The type of the value is not int or long!");
        }
    }

    public static long OptLong(Dictionary<string, object> dic, string key, long fallback = 0)
    {
        if (!dic.ContainsKey(key))
        {
            return fallback;
        }

        if (dic[key].GetType() == typeof(long))
        {
            return (long)dic[key];
        }
        else if (dic[key].GetType() == typeof(int))
        {
            return OptInt(dic, key, (int)fallback);
        }
        else
        {
            throw new Exception("The type of the value is not int or long!");
        }
    }

    public static float OptFloat(Dictionary<string, object> dic, string key, float fallback = 0.0f)
    {
        if (!dic.ContainsKey(key))
        {
            return fallback;
        }

        if (dic[key].GetType() == typeof(float))
        {
            return (float)dic[key];
        }
        else if (dic[key].GetType() == typeof(double))
        {
            return (float)OptDouble(dic, key, fallback);
        }
        else
        {
            throw new Exception("The type of the value is not float or double!");
        }
    }

    public static double OptDouble(Dictionary<string, object> dic, string key, double fallback = 0.0f)
    {
        if (!dic.ContainsKey(key))
        {
            return fallback;
        }

        if (dic[key].GetType() == typeof(double))
        {
            return (double)dic[key];
        }
        else if (dic[key].GetType() == typeof(float))
        {
            return OptFloat(dic, key, (float)fallback);
        }
        else
        {
            throw new Exception("The type of the value is not float or double!");
        }
    }

    public static string OptString(Dictionary<string, object> dic, string key, string fallback = "")
    {
        if (dic.ContainsKey(key))
        {
            return (string)dic[key];
        }
        else
        {
            return fallback;
        }
    }

    public static void Test()
    {
        var jsonString = "{ \"array\": [1.44,2,3], " +
                        "\"string\": \"The quick brown fox \\\"jumps\\\" over the lazy dog \", " +
                        "\"int\": 655, " +
                        "\"float\": 3.1415926, " +
                        "\"bool\": true, " +
                        "\"null\": null }";

        Dictionary<string, object> dict = Json.Deserialize(jsonString) as Dictionary<string, object>;
        string str;

        int iValue = OptInt(dict, "int");
        str = "int: " + iValue + "\n";
        float fValue = OptFloat(dict, "float");
        str += "float: " + fValue + "\n";
        bool bValue = OptBool(dict, "bool");
        str += "bool: " + bValue + "\n";
        string strValue = OptString(dict, "string");
        str += "string: " + strValue + "\n";
        List<object> list = OptList(dict, "array");
        str += "array[0]: " + list[0] + "\n";
        string strNone = OptString(dict, "none", "none");
        str += "none: " + strNone + "\n";
        int iNone = OptInt(dict, "none", -1);
        str += "iNone: " + iNone + "\n";
        DebugUtils.Log(str);
    }

}
