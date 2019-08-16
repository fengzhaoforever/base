using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ResourcePool
{
    #region 单例模式
    private static ResourcePool _instance;
    public static ResourcePool Instance()
    {
        if (_instance == null)
            _instance = new ResourcePool();
        return _instance;
    }
    #endregion
    private Dictionary<UIManager.PanelType, GameObject> _panelDatas;

    private ResourcePool()
    {
        _panelDatas = new Dictionary<UIManager.PanelType, GameObject>();
       
    }
    public GameObject GetPanel(UIManager.PanelType type)
    {
        if (!_panelDatas.ContainsKey(type))
        {
            GameObject panel = Resources.Load<GameObject>("Panel/" + type.ToString());
            GameObject panelObj = UnityEngine.Object.Instantiate(panel);
            //panelObj.transform.localPosition = Vector3.zero;
            _panelDatas.Add(type, panelObj);
        }
        return _panelDatas[type];
    }

}
