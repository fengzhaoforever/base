using UnityEngine;
using System.Collections;
using System;

public enum ConfigType
{
    /// <summary>
    /// 步骤配置
    /// </summary>
    ConfigStep,
    /// <summary>
    /// 描述配置
    /// </summary>
    ConfigStepDesc,
}
public abstract  class ConfigBase
{
    public ConfigType Type;
    public ConfigBase() { }
    public ConfigBase(string className)
    {
        Type = (ConfigType)Enum.Parse(typeof(ConfigType), className);
    }
    public abstract void Set(string[] cells);

    //public abstract ConfigItemBase Get(int id);
}
public abstract class ConfigItemBase
{
    public int id;
    public abstract void Init(string[] args);
}
