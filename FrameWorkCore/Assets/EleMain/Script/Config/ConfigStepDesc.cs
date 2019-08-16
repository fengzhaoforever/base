using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ConfigStepDesc : ConfigBase
{
    public ConfigStepDesc() : base("ConfigStepDesc")
    {

    }
    public Dictionary<int, ConfigStepDescItem> ConfigDatas = new Dictionary<int, ConfigStepDescItem>();
    public override void Set(string[] cells)
    {
        ConfigStepDescItem item = new ConfigStepDescItem();
        item.Init(cells);

        if (!ConfigDatas.ContainsKey(item.id))
        {
            ConfigDatas.Add(item.id, item);
        }
    }
    public ConfigStepDescItem Get(int id)
    {
        if (ConfigDatas.ContainsKey(id))
        {
            return ConfigDatas[id];
        }
        Debug.Log("没找到对应的条目，ID 是:" + id);
        return null;
    }
    public ConfigStepDescItem this[int id]
    {
        get
        {
            return Get(id);
        }
    }
}

public class ConfigStepDescItem : ConfigItemBase
{
    public string Desc;
    public override void Init(string[] args)
    {
        id = int.Parse(args[0]);
        Desc = args[1];
    }
}
