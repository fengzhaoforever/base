using UnityEngine;
using System.Collections.Generic;

public class EleDataBase  {
    private EleDataBase() { InitData(); }
    private static EleDataBase _instance = new EleDataBase();
    public static EleDataBase Instance()
    {
        return _instance;
    }
    private ConfigStep _congfigStep;
    public List<EleItem> EleItems;
    private Dictionary<int, StepItem> StepItems;
    private void InitData()
    {
        _congfigStep = (ConfigStep)ConfigManager.Instance().GetConfig(ConfigType.ConfigStep);
        Transform tran = GameObject.Find("StepObjs").transform;
        EleItems = new List<EleItem>(tran.childCount);
        for (int i = 0; i < tran.childCount; i++)
        {
            EleItem item = new EleItem(tran.GetChild(i));
            EleItems.Add(item);
        }
        StepItems = new Dictionary<int, StepItem>(5);
        
        for (int i = 0; i < _congfigStep.ConfigDatas.Count; i++)
        {
            StepItem sItem = new StepItem();
            sItem.Index = i;
            sItem.Desc = StepManager.Instance().GetCurStepDesc(i);
            sItem.CurDragEleItem = EleItems.Find(x => x.Name.Equals(_congfigStep.Get(i + 1).dragName));
            sItem.CurCastEleItem = EleItems.Find(x => x.Name.Equals(_congfigStep.Get(i + 1).castName));
            StepItems.Add(i, sItem);
        }
        
    }
    public EleItem GetOneEleItem(string name)
    {
        EleItem one = EleItems.Find(x => x.Name == name);
        return one;
    }
    public StepItem GetOneStepItem(int index)
    {
        if(StepItems.ContainsKey(index))
        {
            return StepItems[index];
        }
        Debug.LogError("GetOneStepItem not find index");
        return null;
    }
    public bool IsCurDragTarget(EleItem item)
    {
        int index = StepManager.Instance().CurSetpIndex - 1;
        StepItem temp = GetOneStepItem(index);
        return temp.CurDragEleItem.Name.Equals(item.Name);
    }
    public StepItem GetCurStepItem()
    {
        int index = StepManager.Instance().CurSetpIndex - 1;
        StepItem temp = GetOneStepItem(index);
        return temp;
    }
}
