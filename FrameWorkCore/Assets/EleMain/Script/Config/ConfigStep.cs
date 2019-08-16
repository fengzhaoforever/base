using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ConfigStepItem : ConfigItemBase
{
    public string dragName;
    public string castName;
    public override void Init(string[] args)
    {
        id = int.Parse(args[0]);
        dragName = args[1];
        castName = args[2];
    }
}
public class ConfigStep : ConfigBase
{
    public ConfigStep() : base("ConfigStep")
    {

    }
    public Dictionary<int, ConfigStepItem> ConfigDatas = new Dictionary<int, ConfigStepItem>();

    public override void Set(string[] cells)
    {
        ConfigStepItem item = new ConfigStepItem();
        item.Init(cells);

        if (!ConfigDatas.ContainsKey(item.id))
        {
            ConfigDatas.Add(item.id, item);
        }
    }
    public ConfigStepItem Get(int id)
    {
        if (ConfigDatas.ContainsKey(id))
        {
            return ConfigDatas[id];
        }
        Debug.Log("没找到对应的条目，ID 是:" + id);
        return null;
    }
    //public override  ConfigItemBase Get(int id)
    //{
    //    if (ConfigDatas.ContainsKey(id))
    //    {
    //        return ConfigDatas[id];
    //    }
    //    Debug.Log("没找到对应的条目，ID 是:" + id);
    //    return null;
    //}
    public ConfigStepItem this[int id]
    {
        get
        {
            return Get(id);
        }
    }

}
