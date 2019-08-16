using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

public class ConfigManager  {

    private static ConfigManager _configManager;
    public static ConfigManager Instance()
    {
        if (_configManager == null)
            _configManager = new global::ConfigManager();
        return _configManager;
    }

    public Dictionary<ConfigType, ConfigBase> ConfigDatas = new Dictionary<ConfigType, ConfigBase>();

    
    /// <summary>
    /// 加载txt文件
    /// </summary>
    public void LoadTxt()
    {
        string[] names = Enum.GetNames(typeof(ConfigType));//获取指定枚举中常数名称的数组
        for (int i = 0; i < names.Length; i++)
        {
            TextAsset txtAsset = Resources.Load<TextAsset>("Config/"+ names[i]);

            SplTxt(txtAsset, names[i]);
        }
        
    }
    /// <summary>
    /// 分割字符串
    /// </summary>
    /// <param name="txt"></param>
    public void SplTxt(TextAsset txtAsset,string className)
    {
        string txtTemp = txtAsset.text.Replace("\r", "");
        string[] rows = txtTemp.Split('\n');
        Type type;
        type = Type.GetType(className);//根据字符串获取对应的类    命名空间.类名
        MethodInfo method = type.GetMethod("Set");//获取类里要使用的方法
        object config = Activator.CreateInstance(type);//创建类对应的对象
        for (int i = 1; i < rows.Length; i++)
        {
            string row = rows[i];
            if (string.IsNullOrEmpty(row))
                continue;
            string[] cells = row.Split(' ');
            object[] parameters = new object[] { cells };
            method.Invoke(config, parameters);//调用方法
        }
        SetConfigDatas((ConfigBase)config);
        Resources.UnloadAsset(txtAsset);
    }
    /// <summary>
    /// 储存到容器
    /// </summary>
    public void SetConfigDatas(ConfigBase item)
    {
        if (!ConfigDatas.ContainsKey(item.Type))
        {
            ConfigDatas.Add(item.Type, item);
        }
    }
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public ConfigBase GetConfig(ConfigType type)
    {
        if (ConfigDatas.ContainsKey(type))
        {
            return ConfigDatas[type];
        }
        Debug.Log("没找到对应的配置，type 是:" + type);
        return null;
    }
}
