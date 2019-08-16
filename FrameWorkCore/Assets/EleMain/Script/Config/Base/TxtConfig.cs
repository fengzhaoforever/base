using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TxtConfig<T> where T : TxtConfig<T>, new()
{

    private static Dictionary<string, T> dataDic = new Dictionary<string, T>();

    static TxtConfig()
    {
        Debug.Log("Config/" + typeof(T).Name);
        ParseTable();
    }

    /// <summary>
    /// 解析数据表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tableName"></param>
    public static void ParseTable(System.Action onFinsh = null)
    {
        TextAsset ta = Resources.Load<TextAsset>("Config/" + typeof(T).Name);
        if (ta == null) return;
        //byte[] utf8 = System.Text.UnicodeEncoding.UTF8.GetBytes(ta.text);
        //string configText = System.Text.UnicodeEncoding.UTF8.GetString(utf8);
        string configText = ta.text;
        dataDic.Clear();
        configText = configText.Replace("\r", "");
        string[] configAry = configText.Split('\n');
        for (int i = 1; i < configAry.Length; i++)
        {
            if (configAry[i].Equals("")) continue;
            T data = new T();
            data.Parse(configAry[i].Split(' '));
            dataDic.Add(configAry[i].Split(' ')[0], data);
        }
        if (onFinsh != null)
            onFinsh();
        Resources.UnloadAsset(ta);
    }

    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="ary"></param>
    protected abstract void Parse(string[] ary);

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init() { }

    /// <summary>
    /// 是否包含
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool IsHave(string key)
    {
        return dataDic.ContainsKey(key);
    }

    /// <summary>
    /// 是否包含
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsHave(string key, out T value)
    {
        if (dataDic.ContainsKey(key))
        {
            value = dataDic[key];
            return true;
        }
        value = null;
        return false;
    }

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void TryGet(string key, out T value)
    {
        if (!IsHave(key, out value))
        {
            value = null;
        }
    }

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T TryGet(string key)
    {
        T value = default(T);
        TryGet(key, out value);
        return value;
    }

    /// <summary>
    /// 数量
    /// </summary>
    /// <returns></returns>
    public static int Count()
    {
        return dataDic.Count;
    }

    /// <summary>
    /// 获取
    /// </summary>
    /// <returns></returns>
    public static List<T> TryGet()
    {
        return new List<T>(dataDic.Values);
    }

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="dataList"></param>
    public static void TryGet(out List<T> dataList)
    {
        dataList = TryGet();
    }

}
