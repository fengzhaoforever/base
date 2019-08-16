using System.Collections.Generic;
[System.Serializable]
public class StepItem   {
    /// <summary>
    /// 步骤序号
    /// </summary>
    public int Index;
    private string _desc;
    /// <summary>
    /// 步骤文字描述
    /// </summary>
    public string Desc { get { return _desc; }  set { _desc = value; } }
    ///// <summary>
    ///// 步骤涉及到的物体
    ///// </summary>
    //public List<EleItem> NeedEleItems;
    /// <summary>
    /// 当前步骤需要拖动的物体
    /// </summary>
    public EleItem CurDragEleItem;
    /// <summary>
    /// 当前步骤需要触碰的物体
    /// </summary>
    public EleItem CurCastEleItem;
}
