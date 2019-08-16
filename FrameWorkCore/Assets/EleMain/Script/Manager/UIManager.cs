using UnityEngine;
using System.Collections;

public class UIManager  {
    /// <summary>
    /// 界面类型
    /// </summary>
    public enum PanelType
    {
        /// <summary>
        /// 教学原理图
        /// </summary>
        PanelShowDes,
        /// <summary>
        /// 菜单栏
        /// </summary>
        PanelTopBar,
        /// <summary>
        /// 退出界面
        /// </summary>
        PanelQuit,
        /// <summary>
        /// 实验结束界面
        /// </summary>
        PanelGameOver,
        /// <summary>
        /// 触碰提示界面
        /// </summary>
        PanelTipCast,
    }
    private Transform _uiRoot;
    #region 单例模式
    private static UIManager _instance;
    public static UIManager Instance()
    {
        if (_instance == null)
            _instance = new UIManager();
        return _instance;
    }
    private UIManager()
    {
        _uiRoot = GameObject.Find("UIRoot").transform;
    }

    #endregion

    public void ShowPanel(PanelType type, bool setParent = true, Transform parent = null)
    {
        GameObject go = ResourcePool.Instance().GetPanel(type);
        if (setParent)
        {
            if (parent)
                go.transform.SetParent(parent, false);
            else
                go.transform.SetParent(_uiRoot, false);
        }
        if (go.activeInHierarchy)
            return;
        go.SetActive(true);

    }
    public void HidePanel(PanelType type)
    {
        GameObject go = ResourcePool.Instance().GetPanel(type);
        if (go)
            go.SetActive(false);
    }
}
