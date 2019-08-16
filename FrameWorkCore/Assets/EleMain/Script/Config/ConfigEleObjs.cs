using UnityEngine;
using System.Collections;

public class ConfigEleObjs : TxtConfig<ConfigEleObjs>
{
    ///
    // 键
    ///
    public string id { get; private set; }

    ///
    // 值
    ///
    public string dragName { get; private set; }

    ///
    // 描述
    ///
    public string castName { get; private set; }

    protected override void Parse(string[] ary)
    {
        id = ary[0];
        dragName = ary[1];
        castName = ary[2];
    }
}
