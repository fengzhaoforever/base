using UnityEngine;
using DG.Tweening;
public class EleItem 
{
    public GameObject Go;
    /// <summary>
    /// 名字
    /// </summary>
    public string Name;
    /// <summary>
    /// 摄像机观察位置
    /// </summary>
    public Vector3 LookPos;
    /// <summary>
    /// 摄像机观察角度
    /// </summary>
    public Quaternion LookQua;
    /// <summary>
    /// 初始位置
    /// </summary>
    public Vector3 DefaultPos;
    /// <summary>
    /// 初始角度
    /// </summary>
    public Quaternion DefaultQua;
    public Vector3 TargetMovePos;
    public AnimationClip AniClip;
    private HighlightableObject _hlObj;
    /// <summary>
    /// 是否被射线照射到
    /// </summary>
    private bool _isBeRayCast;
    public bool IsBeRayCast { get { return _isBeRayCast; } set { _isBeRayCast = value; } }
    public EleItem(Transform tran)
    {
        Go = tran.gameObject;
        Name = tran.name;
        LookPos = tran.Find("LookPos").position;
        LookQua = tran.Find("LookPos").rotation;
        DefaultPos = tran.localPosition;
        DefaultQua = tran.localRotation;
        TargetMovePos = Vector3.zero;

        _hlObj = tran.gameObject.AddComponent<HighlightableObject>();
        if (_hlObj == null)
            Debug.LogError("HighlightableObject not find!");
    }

    /// <summary>
    /// 向摄像机移动（变大）并跟随鼠标
    /// </summary>
    public void TweenCloseToCamera(Vector3 pos)
    {
        Go.transform.position = pos;
    }
    /// <summary>
    /// 移动到触碰物体预设点（触碰成果后调用）
    /// 移动到初始位置（未触碰物体、播放动画结束后调用）
    /// </summary>
    public void TweenToPos(Vector3 pos, TweenCallback callBack=null)
    {
        Go.transform.DOMove(pos, 0.5f).onComplete+=callBack;
    }
    /// <summary>
    /// 播放动画
    /// </summary>
    public void PlayAni()
    {
        Go.transform.DORotate(Vector3.right, 2).OnComplete(() => {Go.transform.DORotateQuaternion(DefaultQua,0.5f).OnStart(() => TweenToPos(DefaultPos,()=> StepManager.Instance().StepToNext())); });
    }
    /// <summary>
    /// 显示高亮
    /// </summary>
    /// <param name="flashing">是否为闪烁</param>
    public void SetHighLighting(bool flashing)
    {
        if (flashing)
        {
            //闪烁
            _hlObj.FlashingOn(Color.red, Color.green, 2);
        }
        else
        {
            //常亮
            _hlObj.ConstantOn(Color.black);
        }
    }
    /// <summary>
    /// 关闭高亮效果
    /// </summary>
    public void OffHighLighting()
    {
        _hlObj.Off();
    }
}
